using Api.Dtos;
using Application.Lessons.Commands;
using Domain.Courses;
using Domain.Lessons;
using Domain.Roles.Role;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Courses;
using Tests.Data.Lessons;
using Tests.Data.Users;

namespace Tests.Api
{
    public class LessonControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private const string BaseRoute = "http://localhost:5078/lessons";

        private Lesson _firstTestLesson;
        private Lesson _secondTestLesson;
        private Role _testRole;
        private User _testAuthor;
        private Course _testCourse;

        public LessonControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

        public async Task InitializeAsync()
        {
            _testRole = Role.New("user");
            await Context.Roles.AddAsync(_testRole);
            await SaveChangesAsync();

            _testAuthor = UserData.FirstUser(_testRole.Id);
            await Context.Users.AddAsync(_testAuthor);
            await SaveChangesAsync();

            _testCourse = CourseData.FirstCourse(_testAuthor.Id);
            await Context.Courses.AddAsync(_testCourse);
            await SaveChangesAsync();

            _firstTestLesson = LessonData.FirstLesson(_testCourse.Id);
            _secondTestLesson = LessonData.SecondLesson(_testCourse.Id);
            await Context.Lessons.AddAsync(_firstTestLesson);
            await Context.Lessons.AddAsync(_secondTestLesson);
            await SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.Lessons.RemoveRange(Context.Lessons);
            Context.Courses.RemoveRange(Context.Courses);
            Context.Users.RemoveRange(Context.Users);
            Context.Roles.RemoveRange(Context.Roles);
            await SaveChangesAsync();
        }

        #region GET Tests

        [Fact]
        public async Task ShouldGetAllLessons()
        {
            var response = await Client.GetAsync(BaseRoute);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var lessons = await response.ToResponseModel<List<LessonDto>>();
            lessons.Should().NotBeNull();
            lessons.Count.Should().BeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task ShouldGetLessonById()
        {
            var response = await Client.GetAsync($"{BaseRoute}/{_firstTestLesson.Id.Value}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var lessonDto = await response.ToResponseModel<LessonDto>();
            lessonDto.Should().NotBeNull();
            lessonDto!.Id.Value.Should().Be(_firstTestLesson.Id.Value);
            lessonDto.Title.Should().Be(_firstTestLesson.Title);
            lessonDto.Content.Should().Be(_firstTestLesson.Content);
            lessonDto.Order.Should().Be(_firstTestLesson.Order);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenLessonDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST Tests

        [Fact]
        public async Task ShouldCreateLesson()
        {
            var request = new CreateLessonDto(
                Title: "New Test Lesson",
                Content: "Lesson Content",
                CourseId: _testCourse.Id,
                Order: 3
            );

            var response = await Client.PostAsJsonAsync(BaseRoute, request);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var lessonDto = await response.ToResponseModel<LessonDto>();
            lessonDto.Should().NotBeNull();
            lessonDto!.Title.Should().Be(request.Title);
            lessonDto.Content.Should().Be(request.Content);
            lessonDto.CourseId.Should().Be(request.CourseId);
            lessonDto.Order.Should().Be(request.Order);
            lessonDto.Id.Should().NotBeNull();

            var dbLesson = await Context.Lessons
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == lessonDto.Id);

            dbLesson.Should().NotBeNull();
            dbLesson!.Title.Should().Be(request.Title);
            dbLesson.Content.Should().Be(request.Content);
            dbLesson.CourseId.Should().Be(request.CourseId);
            dbLesson.Order.Should().Be(request.Order);
        }

        #endregion

        #region PUT Tests

        [Fact]
        public async Task ShouldUpdateLesson()
        {
            var request = new UpdateLessonDto(
                Id: _firstTestLesson.Id.Value,
                Title: "Updated Title",
                Content: "Updated Content",
                Order: 10
            );

            var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestLesson.Id.Value}", request);
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var updatedLesson = await Context.Lessons
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == _firstTestLesson.Id);

            updatedLesson.Should().NotBeNull();
            updatedLesson!.Title.Should().Be(request.Title);
            updatedLesson.Content.Should().Be(request.Content);
            updatedLesson.Order.Should().Be(request.Order);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenUpdatingNonExistentLesson()
        {
            var nonExistentId = Guid.NewGuid();

            var request = new UpdateLessonDto(
                Id: nonExistentId,
                Title: "Nonexistent",
                Content: "Nonexistent Content",
                Order: 1
            );

            var response = await Client.PutAsJsonAsync($"{BaseRoute}/{nonExistentId}", request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE Tests

        [Fact]
        public async Task ShouldDeleteLesson()
        {
            var response = await Client.DeleteAsync($"{BaseRoute}/{_secondTestLesson.Id.Value}");
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var lessonExists = await Context.Lessons
                .AnyAsync(l => l.Id == _secondTestLesson.Id);
            lessonExists.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenDeletingNonExistentLesson()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await Client.DeleteAsync($"{BaseRoute}/{nonExistentId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}