using Api.Dtos;
using Domain.Roles;
using Domain.Roles.Role;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Roles;

namespace Tests.Api
{
    public class RoleControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private const string BaseRoute = "http://localhost:5078/roles";

        private Role _firstTestRole = null!;
        private Role _secondTestRole = null!;

        public RoleControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

        public async Task InitializeAsync()
        {
            _firstTestRole = RoleData.FirstRole();
            _secondTestRole = RoleData.SecondRole();
            await Context.Roles.AddAsync(_firstTestRole);
            await Context.Roles.AddAsync(_secondTestRole);
            await SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.Roles.RemoveRange(Context.Roles);
            await SaveChangesAsync();
        }

        #region GET Tests

        [Fact]
        public async Task ShouldGetAllRoles()
        {
            var response = await Client.GetAsync(BaseRoute);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var roles = await response.ToResponseModel<List<RoleDto>>();
            roles.Should().NotBeNull();
            roles.Count.Should().BeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task ShouldGetRoleById()
        {
            var response = await Client.GetAsync($"{BaseRoute}/{_firstTestRole.Id.Value}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var roleDto = await response.ToResponseModel<RoleDto>();
            roleDto.Should().NotBeNull();
            roleDto!.Id.Value.Should().Be(_firstTestRole.Id.Value);
            roleDto.Name.Should().Be(_firstTestRole.Name);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenRoleDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST Tests

        [Fact]
        public async Task ShouldCreateRole()
        {
            var request = new CreateRoleDto(
                Name: "New Test Role"
            );

            var response = await Client.PostAsJsonAsync(BaseRoute, request);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var roleDto = await response.ToResponseModel<RoleDto>();
            roleDto.Should().NotBeNull();
            roleDto!.Name.Should().Be(request.Name);
            roleDto.Id.Should().NotBeNull();

            var dbRole = await Context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roleDto.Id);

            dbRole.Should().NotBeNull();
            dbRole!.Name.Should().Be(request.Name);
        }

        #endregion

        #region PUT Tests

        [Fact]
        public async Task ShouldUpdateRole()
        {
            var request = new UpdateRoleDto(
                Id: _firstTestRole.Id.Value,
                Name: "Updated Role Name"
            );

            var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestRole.Id.Value}", request);
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var updatedRole = await Context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == _firstTestRole.Id);

            updatedRole.Should().NotBeNull();
            updatedRole!.Name.Should().Be(request.Name);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenUpdatingNonExistentRole()
        {
            var nonExistentId = Guid.NewGuid();

            var request = new UpdateRoleDto(
                Id: nonExistentId,
                Name: "Nonexistent"
            );

            var response = await Client.PutAsJsonAsync($"{BaseRoute}/{nonExistentId}", request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE Tests

        [Fact]
        public async Task ShouldDeleteRole()
        {
            var response = await Client.DeleteAsync($"{BaseRoute}/{_secondTestRole.Id.Value}");
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var roleExists = await Context.Roles
                .AnyAsync(r => r.Id == _secondTestRole.Id);
            roleExists.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenDeletingNonExistentRole()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await Client.DeleteAsync($"{BaseRoute}/{nonExistentId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}