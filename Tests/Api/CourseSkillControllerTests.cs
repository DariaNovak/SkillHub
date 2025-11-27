using Api.Dtos;
using Domain.CoursesSkills;
using Domain.Courses;
using Domain.Skills;
using Domain.Users;
using Domain.Roles.Role;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.CoursesSkills;
using Tests.Data.Courses;
using Tests.Data.Skills;
using Tests.Data.Users;
using Tests.Data.Roles;

namespace Tests.Api
{
    public class CourseSkillControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private const string BaseRoute = "http://localhost:5078/courseskills";
        private const string AllCourseSkillsRoute = "http://localhost:5078/courseskills";

        private Role _testRole = null!;
        private User _testAuthor = null!;
        private Course _testCourse = null!;
        private Skill _firstTestSkill = null!;
        private Skill _secondTestSkill = null!;
        private CourseSkill _firstTestCourseSkill = null!;
        private CourseSkill _secondTestCourseSkill = null!;

        public CourseSkillControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

        public async Task InitializeAsync()
        {
            // Create test role
            _testRole = RoleData.FirstRole();
            await Context.Roles.AddAsync(_testRole);
            await SaveChangesAsync();

            // Create test author
            _testAuthor = UserData.FirstUser(_testRole.Id);
            await Context.Users.AddAsync(_testAuthor);
            await SaveChangesAsync();

            // Create test course
            _testCourse = CourseData.FirstCourse(_testAuthor.Id);
            await Context.Courses.AddAsync(_testCourse);

            // Create test skills
            _firstTestSkill = SkillData.FirstSkill();
            _secondTestSkill = SkillData.SecondSkill();
            await Context.Skills.AddAsync(_firstTestSkill);
            await Context.Skills.AddAsync(_secondTestSkill);
            
            await SaveChangesAsync();

            // Create test course skills
            _firstTestCourseSkill = CourseSkillData.FirstCourseSkill(_testCourse, _firstTestSkill);
            _secondTestCourseSkill = CourseSkillData.SecondCourseSkill(_testCourse, _secondTestSkill);
            await Context.CourseSkills.AddAsync(_firstTestCourseSkill);
            await Context.CourseSkills.AddAsync(_secondTestCourseSkill);
            
            await SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.CourseSkills.RemoveRange(Context.CourseSkills);
            Context.Courses.RemoveRange(Context.Courses);
            Context.Skills.RemoveRange(Context.Skills);
            Context.Users.RemoveRange(Context.Users);
            Context.Roles.RemoveRange(Context.Roles);
            await SaveChangesAsync();
        }

        #region GET Tests

        [Fact]
        public async Task ShouldGetAllCourseSkills()
        {
            // Arrange
            // Test data is set up in InitializeAsync

            // Act
            var response = await Client.GetAsync(AllCourseSkillsRoute);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var courseSkills = await response.ToResponseModel<List<CourseSkillDto>>();
            courseSkills.Should().NotBeNull();
            courseSkills!.Count.Should().BeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task ShouldGetCourseSkillById()
        {
            // Arrange
            // Test data is set up in InitializeAsync

            // Act
            var response = await Client.GetAsync(
                $"{BaseRoute}/{_testCourse.Id.Value}/skills/{_firstTestSkill.Id.Value}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var courseSkillDto = await response.ToResponseModel<CourseSkillDto>();
            courseSkillDto.Should().NotBeNull();
            courseSkillDto!.CourseId.Value.Should().Be(_testCourse.Id.Value);
            courseSkillDto.SkillId.Value.Should().Be(_firstTestSkill.Id.Value);
        }

        [Fact]
        public async Task ShouldGetCourseSkillsByCourseId()
        {
            // Arrange
            // Test data is set up in InitializeAsync

            // Act
            var response = await Client.GetAsync($"{BaseRoute}/{_testCourse.Id.Value}/skills");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var courseSkills = await response.ToResponseModel<List<CourseSkillDto>>();
            courseSkills.Should().NotBeNull();
            courseSkills!.Count.Should().Be(2);
            courseSkills.All(cs => cs.CourseId.Value == _testCourse.Id.Value).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenCourseSkillDoesNotExist()
        {
            // Arrange
            var nonExistentSkillId = Guid.NewGuid();

            // Act
            var response = await Client.GetAsync(
                $"{BaseRoute}/{_testCourse.Id.Value}/skills/{nonExistentSkillId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenCourseDoesNotExist()
        {
            // Arrange
            var nonExistentCourseId = Guid.NewGuid();

            // Act
            var response = await Client.GetAsync(
                $"{BaseRoute}/{nonExistentCourseId}/skills/{_firstTestSkill.Id.Value}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST Tests

        [Fact]
        public async Task ShouldCreateCourseSkill()
        {
            // Arrange
            var newSkill = SkillData.ThirdSkill();
            await Context.Skills.AddAsync(newSkill);
            await SaveChangesAsync();

            var request = new CreateCourseSkillDto(
                CourseId: _testCourse.Id.Value,
                SkillId: newSkill.Id.Value
            );

            // Act
            var response = await Client.PostAsJsonAsync(
                $"{BaseRoute}/{_testCourse.Id.Value}/skills", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var courseSkillDto = await response.ToResponseModel<CourseSkillDto>();
            courseSkillDto.Should().NotBeNull();
            courseSkillDto!.CourseId.Value.Should().Be(request.CourseId);
            courseSkillDto.SkillId.Value.Should().Be(request.SkillId);

            var dbCourseSkill = await Context.CourseSkills
                .AsNoTracking()
                .FirstOrDefaultAsync(cs => 
                    cs.CourseId == _testCourse.Id && 
                    cs.SkillId == newSkill.Id);

            dbCourseSkill.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldNotCreateDuplicateCourseSkill()
        {
            // Arrange
            var request = new CreateCourseSkillDto(
                CourseId: _testCourse.Id.Value,
                SkillId: _firstTestSkill.Id.Value  // Already exists
            );

            // Act
            var response = await Client.PostAsJsonAsync(
                $"{BaseRoute}/{_testCourse.Id.Value}/skills", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task ShouldNotCreateCourseSkillWithNonExistentCourse()
        {
            // Arrange
            var nonExistentCourseId = Guid.NewGuid();
            var request = new CreateCourseSkillDto(
                CourseId: nonExistentCourseId,
                SkillId: _firstTestSkill.Id.Value
            );

            // Act
            var response = await Client.PostAsJsonAsync(
                $"{BaseRoute}/{nonExistentCourseId}/skills", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task ShouldNotCreateCourseSkillWithNonExistentSkill()
        {
            // Arrange
            var nonExistentSkillId = Guid.NewGuid();
            var request = new CreateCourseSkillDto(
                CourseId: _testCourse.Id.Value,
                SkillId: nonExistentSkillId
            );

            // Act
            var response = await Client.PostAsJsonAsync(
                $"{BaseRoute}/{_testCourse.Id.Value}/skills", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        #endregion

        #region DELETE Tests

        [Fact]
        public async Task ShouldDeleteCourseSkill()
        {
            // Arrange
            // Test data is set up in InitializeAsync

            // Act
            var response = await Client.DeleteAsync(
                $"{BaseRoute}/{_testCourse.Id.Value}/skills/{_secondTestSkill.Id.Value}");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var courseSkillExists = await Context.CourseSkills
                .AnyAsync(cs => 
                    cs.CourseId == _testCourse.Id && 
                    cs.SkillId == _secondTestSkill.Id);
            
            courseSkillExists.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenDeletingNonExistentCourseSkill()
        {
            // Arrange
            var nonExistentSkillId = Guid.NewGuid();

            // Act
            var response = await Client.DeleteAsync(
                $"{BaseRoute}/{_testCourse.Id.Value}/skills/{nonExistentSkillId}");
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenDeletingFromNonExistentCourse()
        {
            // Arrange
            var nonExistentCourseId = Guid.NewGuid();

            // Act
            var response = await Client.DeleteAsync(
                $"{BaseRoute}/{nonExistentCourseId}/skills/{_firstTestSkill.Id.Value}");
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region Additional Tests

        [Fact]
        public async Task ShouldGetEmptyListWhenCourseHasNoSkills()
        {
            // Arrange
            var newAuthor = UserData.WithCustomData("New Author", "newauthor@test.com", "password", _testRole.Id);
            await Context.Users.AddAsync(newAuthor);
            await SaveChangesAsync();

            var newCourse = CourseData.WithCustomData(
                CourseId.New(),
                "New Course", 
                "Course without skills", 
                newAuthor.Id);
            await Context.Courses.AddAsync(newCourse);
            await SaveChangesAsync();

            // Act
            var response = await Client.GetAsync($"{BaseRoute}/{newCourse.Id.Value}/skills");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldManageMultipleSkillsForCourse()
        {
            // Arrange
            var skill3 = SkillData.ThirdSkill();
            var skill4 = SkillData.WithCustomData("Kubernetes");
            await Context.Skills.AddRangeAsync(skill3, skill4);
            await SaveChangesAsync();

            // Act
            // Add first skill
            var request1 = new CreateCourseSkillDto(_testCourse.Id.Value, skill3.Id.Value);
            var response1 = await Client.PostAsJsonAsync(
                $"{BaseRoute}/{_testCourse.Id.Value}/skills", request1);

            // Add second skill
            var request2 = new CreateCourseSkillDto(_testCourse.Id.Value, skill4.Id.Value);
            var response2 = await Client.PostAsJsonAsync(
                $"{BaseRoute}/{_testCourse.Id.Value}/skills", request2);

            // Verify both skills are added
            var getResponse = await Client.GetAsync($"{BaseRoute}/{_testCourse.Id.Value}/skills");

            // Assert
            response1.IsSuccessStatusCode.Should().BeTrue();
            response2.IsSuccessStatusCode.Should().BeTrue();

            var courseSkills = await getResponse.ToResponseModel<List<CourseSkillDto>>();
            courseSkills!.Count.Should().Be(4); // 2 initial + 2 new
        }

        #endregion
    }
}
