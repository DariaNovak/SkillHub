using Api.Dtos;
using Domain.UsersSkills;
using Domain.Users;
using Domain.Skills;
using Domain.Roles.Role;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.UsersSkills;
using Tests.Data.Users;
using Tests.Data.Skills;
using Tests.Data.Roles;

namespace Tests.Api
{
    public class UserSkillControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private const string BaseRoute = "http://localhost:5078/userskills";
        private const string AllUserSkillsRoute = "http://localhost:5078/userskills";

        private Role _testRole = null!;
        private User _testUser = null!;
        private Skill _firstTestSkill = null!;
        private Skill _secondTestSkill = null!;
        private UserSkill _firstTestUserSkill = null!;
        private UserSkill _secondTestUserSkill = null!;

        public UserSkillControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

        public async Task InitializeAsync()
        {
            // Create test role
            _testRole = RoleData.FirstRole();
            await Context.Roles.AddAsync(_testRole);
            await SaveChangesAsync();

            // Create test user
            _testUser = UserData.FirstUser(_testRole.Id);
            await Context.Users.AddAsync(_testUser);

            // Create test skills
            _firstTestSkill = SkillData.FirstSkill();
            _secondTestSkill = SkillData.SecondSkill();
            await Context.Skills.AddAsync(_firstTestSkill);
            await Context.Skills.AddAsync(_secondTestSkill);
            
            await SaveChangesAsync();

            // Create test user skills
            _firstTestUserSkill = UserSkillData.FirstUserSkill(_testUser.Id, _firstTestSkill.Id);
            _secondTestUserSkill = UserSkillData.SecondUserSkill(_testUser.Id, _secondTestSkill.Id);
            await Context.UserSkills.AddAsync(_firstTestUserSkill);
            await Context.UserSkills.AddAsync(_secondTestUserSkill);
            
            await SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.UserSkills.RemoveRange(Context.UserSkills);
            Context.Skills.RemoveRange(Context.Skills);
            Context.Users.RemoveRange(Context.Users);
            Context.Roles.RemoveRange(Context.Roles);
            await SaveChangesAsync();
        }

        #region GET Tests

        [Fact]
        public async Task ShouldGetAllUserSkills()
        {
            // Arrange
            // Test data is set up in InitializeAsync

            // Act
            var response = await Client.GetAsync(AllUserSkillsRoute);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var userSkills = await response.ToResponseModel<List<UserSkillDto>>();
            userSkills.Should().NotBeNull();
            userSkills!.Count.Should().BeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task ShouldGetUserSkillById()
        {
            // Arrange
            // Test data is set up in InitializeAsync

            // Act
            var response = await Client.GetAsync(
                $"{BaseRoute}/{_testUser.Id.Value}/skills/{_firstTestSkill.Id.Value}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var userSkillDto = await response.ToResponseModel<UserSkillDto>();
            userSkillDto.Should().NotBeNull();
            userSkillDto!.UserId.Value.Should().Be(_testUser.Id.Value);
            userSkillDto.SkillId.Value.Should().Be(_firstTestSkill.Id.Value);
            userSkillDto.ProficiencyLevel.Should().Be(4);
        }

        [Fact]
        public async Task ShouldGetUserSkillsByUserId()
        {
            // Arrange
            // Test data is set up in InitializeAsync

            // Act
            var response = await Client.GetAsync($"{BaseRoute}/{_testUser.Id.Value}/skills");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var userSkills = await response.ToResponseModel<List<UserSkillDto>>();
            userSkills.Should().NotBeNull();
            userSkills!.Count.Should().Be(2);
            userSkills.All(us => us.UserId.Value == _testUser.Id.Value).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenUserSkillDoesNotExist()
        {
            // Arrange
            var nonExistentSkillId = Guid.NewGuid();

            // Act
            var response = await Client.GetAsync(
                $"{BaseRoute}/{_testUser.Id.Value}/skills/{nonExistentSkillId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST Tests

        [Fact]
        public async Task ShouldCreateUserSkill()
        {
            // Arrange
            var newSkill = SkillData.ThirdSkill();
            await Context.Skills.AddAsync(newSkill);
            await SaveChangesAsync();

            var request = new CreateUserSkillDto(
                UserId: _testUser.Id.Value,
                SkillId: newSkill.Id.Value,
                ProficiencyLevel: 5
            );

            // Act
            var response = await Client.PostAsJsonAsync(
                $"{BaseRoute}/{_testUser.Id.Value}/skills", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var userSkillDto = await response.ToResponseModel<UserSkillDto>();
            userSkillDto.Should().NotBeNull();
            userSkillDto!.UserId.Value.Should().Be(request.UserId);
            userSkillDto.SkillId.Value.Should().Be(request.SkillId);
            userSkillDto.ProficiencyLevel.Should().Be(request.ProficiencyLevel);

            var dbUserSkill = await Context.UserSkills
                .AsNoTracking()
                .FirstOrDefaultAsync(us => 
                    us.UserId == _testUser.Id && 
                    us.SkillId == newSkill.Id);

            dbUserSkill.Should().NotBeNull();
            dbUserSkill!.ProficiencyLevel.Should().Be(request.ProficiencyLevel);
        }

        [Fact]
        public async Task ShouldCreateUserSkillWithDefaultProficiencyLevel()
        {
            // Arrange
            var newSkill = SkillData.WithCustomData("Docker");
            await Context.Skills.AddAsync(newSkill);
            await SaveChangesAsync();

            var request = new CreateUserSkillDto(
                UserId: _testUser.Id.Value,
                SkillId: newSkill.Id.Value,
                ProficiencyLevel: 3
            );

            // Act
            var response = await Client.PostAsJsonAsync(
                $"{BaseRoute}/{_testUser.Id.Value}/skills", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var userSkillDto = await response.ToResponseModel<UserSkillDto>();
            userSkillDto!.ProficiencyLevel.Should().Be(3);
        }

        #endregion

        #region PUT Tests

        [Fact]
        public async Task ShouldUpdateUserSkill()
        {
            // Arrange
            var request = new UpdateUserSkillDto(
                UserId: _testUser.Id.Value,
                SkillId: _firstTestSkill.Id.Value,
                ProficiencyLevel: 5
            );

            // Act
            var response = await Client.PutAsJsonAsync(
                $"{BaseRoute}/{_testUser.Id.Value}/skills/{_firstTestSkill.Id.Value}", 
                request);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedUserSkill = await Context.UserSkills
                .AsNoTracking()
                .FirstOrDefaultAsync(us => 
                    us.UserId == _testUser.Id && 
                    us.SkillId == _firstTestSkill.Id);

            updatedUserSkill.Should().NotBeNull();
            updatedUserSkill!.ProficiencyLevel.Should().Be(5);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenUpdatingNonExistentUserSkill()
        {
            // Arrange
            var nonExistentSkillId = Guid.NewGuid();

            var request = new UpdateUserSkillDto(
                UserId: _testUser.Id.Value,
                SkillId: nonExistentSkillId,
                ProficiencyLevel: 4
            );

            // Act
            var response = await Client.PutAsJsonAsync(
                $"{BaseRoute}/{_testUser.Id.Value}/skills/{nonExistentSkillId}", 
                request);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE Tests

        [Fact]
        public async Task ShouldDeleteUserSkill()
        {
            // Arrange
            // Test data is set up in InitializeAsync

            // Act
            var response = await Client.DeleteAsync(
                $"{BaseRoute}/{_testUser.Id.Value}/skills/{_secondTestSkill.Id.Value}");
            
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var userSkillExists = await Context.UserSkills
                .AnyAsync(us => 
                    us.UserId == _testUser.Id && 
                    us.SkillId == _secondTestSkill.Id);
            
            userSkillExists.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenDeletingNonExistentUserSkill()
        {
            // Arrange
            var nonExistentSkillId = Guid.NewGuid();

            // Act
            var response = await Client.DeleteAsync(
                $"{BaseRoute}/{_testUser.Id.Value}/skills/{nonExistentSkillId}");
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}
