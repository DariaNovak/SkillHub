using Api.Dtos;
using Application.Courses.Commands;
using Domain.Courses;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Courses;
using Tests.Data.Users;

namespace Tests.Api
{
    public class CourseControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private const string BaseRoute = "http://localhost:5078/courses";

        private readonly Course _firstTestCourse;
        private readonly Course _secondTestCourse;

        public CourseControllerTests(IntegrationTestWebFactory factory) : base(factory)
        {
            _firstTestCourse = CourseData.FirstCourse();
            _secondTestCourse = CourseData.SecondCourse();
        }

        public async Task InitializeAsync()
        {
            await Context.Courses.AddAsync(_firstTestCourse);
            await Context.Courses.AddAsync(_secondTestCourse);
            await SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.Courses.RemoveRange(Context.Courses);
            Context.Users.RemoveRange(Context.Users);
            await SaveChangesAsync();
        }

        #region GET Tests

        [Fact]
        public async Task ShouldGetAllCourses()
        {
            var response = await Client.GetAsync(BaseRoute);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var courses = await response.ToResponseModel<List<CourseDto>>();
            courses.Should().NotBeNull();
            courses.Count.Should().BeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task ShouldGetCourseById()
        {
            var response = await Client.GetAsync($"{BaseRoute}/{_firstTestCourse.Id.Value}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var courseDto = await response.ToResponseModel<CourseDto>();
            courseDto.Should().NotBeNull();
            courseDto!.Id.Value.Should().Be(_firstTestCourse.Id.Value);
            courseDto.Title.Should().Be(_firstTestCourse.Title);
            courseDto.Description.Should().Be(_firstTestCourse.Description);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenCourseDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST Tests

        [Fact]
        public async Task ShouldCreateCourse()
        {

            var request = new CreateCourseCommand
            {
                Id = CourseId.New(),
                Title = "New Test Course",
                Description = "Course Description",
                AuthorId = UserId.New()
            };

            var response = await Client.PostAsJsonAsync(BaseRoute, request);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var courseDto = await response.ToResponseModel<CourseDto>();
            courseDto.Should().NotBeNull();
            courseDto!.Title.Should().Be(request.Title);

            var dbCourse = await Context.Courses
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            dbCourse.Should().NotBeNull();
            dbCourse!.Title.Should().Be(request.Title);
            dbCourse.Description.Should().Be(request.Description);
            dbCourse.AuthorId.Should().Be(request.AuthorId);
        }

        #endregion

        #region PUT Tests

        [Fact]
        public async Task ShouldUpdateCourse()
        {
            var request = new UpdateCourseCommand
            {
                CourseId = _firstTestCourse.Id,
                Title = "Updated Title",
                Description = "Updated Description",
                AuthorId = UserId.New()
            };

            var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestCourse.Id.Value}", request);
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var updatedCourse = await Context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == _firstTestCourse.Id);

            updatedCourse.Should().NotBeNull();
            updatedCourse!.Title.Should().Be(request.Title);
            updatedCourse.Description.Should().Be(request.Description);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenUpdatingNonExistentCourse()
        {
            var nonExistentId = Guid.NewGuid();
            var request = new UpdateCourseCommand
            {
                CourseId = new CourseId(nonExistentId),
                Title = "Nonexistent",
                Description = "None",
                AuthorId = UserId.New()
            };

            var response = await Client.PutAsJsonAsync($"{BaseRoute}/{nonExistentId}", request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE Tests

        [Fact]
        public async Task ShouldDeleteCourse()
        {
            var response = await Client.DeleteAsync($"{BaseRoute}/{_secondTestCourse.Id.Value}");
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var courseExists = await Context.Courses
                .AnyAsync(c => c.Id == _secondTestCourse.Id);
            courseExists.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenDeletingNonExistentCourse()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await Client.DeleteAsync($"{BaseRoute}/{nonExistentId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}
