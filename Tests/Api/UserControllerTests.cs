using Api.Dtos;
using Application.Users.Commands;
using Domain.Roles;
using Domain.Roles.Role;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Users;

namespace Tests.Api
{
    public class UserControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private const string BaseRoute = "http://localhost:5078/users";

        private readonly User _firstTestUser;
        private readonly User _secondTestUser;
        private readonly Role testRole;

        public UserControllerTests(IntegrationTestWebFactory factory) : base(factory)
        {
            testRole = Role.New("user");
            _firstTestUser = UserData.FirstUser(testRole.Id);
            _secondTestUser = UserData.SecondUser(testRole.Id);
        }

        public async Task InitializeAsync()
        {
            await Context.Roles.AddAsync(testRole);
            await Context.SaveChangesAsync();

            await Context.Users.AddAsync(_firstTestUser);
            await Context.Users.AddAsync(_secondTestUser);
            await SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.Users.RemoveRange(Context.Users);
            await SaveChangesAsync();
        }

        #region GET Tests

        [Fact]
        public async Task ShouldGetAllUsers()
        {
            var response = await Client.GetAsync(BaseRoute);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var users = await response.ToResponseModel<List<UserDto>>();
            users.Should().NotBeNull();
            users.Count.Should().BeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task ShouldGetUserById()
        {
            var response = await Client.GetAsync($"{BaseRoute}/{_firstTestUser.Id.Value}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var userDto = await response.ToResponseModel<UserDto>();
            userDto.Should().NotBeNull();
            userDto!.Id.Value.Should().Be(_firstTestUser.Id.Value);
            userDto.Name.Should().Be(_firstTestUser.Name);
            userDto.Email.Should().Be(_firstTestUser.Email);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenUserDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST Tests

        [Fact]
        public async Task ShouldCreateUser()
        {
            var existingRole = await Context.Roles.FirstOrDefaultAsync();
            if (existingRole == null)
            {
                existingRole = Role.New("admin");
                Context.Roles.Add(existingRole);
                await Context.SaveChangesAsync();
            }

            var roleExists = await Context.Roles.AnyAsync(r => r.Id == existingRole.Id);
            roleExists.Should().BeTrue("role must exist in DB before creating user");

            var request = new CreateUserDto(
                Name: "New Test User",
                Email: "new.user@example.com",
                PasswordHash: "Secure123!",
                RoleId: existingRole.Id.Value,
                JoinDate: DateTime.UtcNow
            );

            var response = await Client.PostAsJsonAsync(BaseRoute, request);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var userDto = await response.ToResponseModel<UserDto>();
            userDto.Should().NotBeNull();
            userDto!.Name.Should().Be(request.Name);

            var dbUser = await Context.Users
                .FirstOrDefaultAsync(u => u.Id == new UserId(userDto.Id.Value));

            dbUser.Should().NotBeNull();
            dbUser!.Name.Should().Be(request.Name);
            dbUser.Email.Should().Be(request.Email);
            dbUser.RoleId.Should().Be(existingRole.Id);
        }

        [Fact]
        public async Task ShouldNotCreateUserWithEmptyEmail()
        {
            var request = new CreateUserCommand
            {
                Name = "Invalid User",
                Email = "",
                PasswordHash = "123",
                RoleId = new RoleId(Guid.NewGuid()),
                JoinDate = DateTime.UtcNow
            };

            var response = await Client.PostAsJsonAsync(BaseRoute, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region PUT Tests

        [Fact]
        public async Task ShouldUpdateUser()
        {
            var existingRole = await Context.Roles.FirstOrDefaultAsync();
            if (existingRole == null)
            {
                existingRole = Role.New("admin");
                Context.Roles.Add(existingRole);
                await Context.SaveChangesAsync();
            }

            var roleExists = await Context.Roles.AnyAsync(r => r.Id == existingRole.Id);
            roleExists.Should().BeTrue("role must exist in DB before creating user");

            var request = new UpdateUserDto(
                Id: _firstTestUser.Id.Value,
                Name: "Updated Name",
                Email: "updated.email@example.com",
                PasswordHash: "UpdatedPass123!",
                RoleId: existingRole.Id.Value,
                JoinDate: DateTime.UtcNow
            );

            var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestUser.Id.Value}", request);
            response.IsSuccessStatusCode.Should().BeTrue();

            var updatedUser = await Context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == _firstTestUser.Id);

            updatedUser.Should().NotBeNull();
            updatedUser!.Name.Should().Be(request.Name);
            updatedUser.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenUpdatingNonExistentUser()
        {
            var nonExistentId = Guid.NewGuid();
            var request = new UpdateUserDto(
                Id: nonExistentId,
                Name: "Nonexistent",
                Email: "none@example.com",
                PasswordHash: "123456",
                RoleId: Guid.NewGuid(),
                JoinDate: DateTime.UtcNow
            );

            var response = await Client.PutAsJsonAsync($"{BaseRoute}/{nonExistentId}", request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        #endregion

        #region DELETE Tests

        [Fact]
        public async Task ShouldDeleteUser()
        {
            var response = await Client.DeleteAsync($"{BaseRoute}/{_secondTestUser.Id.Value}");
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var userExists = await Context.Users
                .AnyAsync(u => u.Id == _secondTestUser.Id);
            userExists.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenDeletingNonExistentUser()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await Client.DeleteAsync($"{BaseRoute}/{nonExistentId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}
