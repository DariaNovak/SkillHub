using Api.Dtos;
using Domain.Skills;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Skills;

namespace Tests.Api
{
    public class SkillControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private const string BaseRoute = "http://localhost:5078/skills";

        private Skill _firstTestSkill = null!;
        private Skill _secondTestSkill = null!;

        public SkillControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

        public async Task InitializeAsync()
        {
            _firstTestSkill = SkillData.FirstSkill();
            _secondTestSkill = SkillData.SecondSkill();
            await Context.Skills.AddAsync(_firstTestSkill);
            await Context.Skills.AddAsync(_secondTestSkill);
            await SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.Skills.RemoveRange(Context.Skills);
            await SaveChangesAsync();
        }

        #region GET Tests

        [Fact]
        public async Task ShouldGetAllSkills()
        {
            var response = await Client.GetAsync(BaseRoute);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var skills = await response.ToResponseModel<List<SkillDto>>();
            skills.Should().NotBeNull();
            skills.Count.Should().BeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task ShouldGetSkillById()
        {
            var response = await Client.GetAsync($"{BaseRoute}/{_firstTestSkill.Id.Value}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var skillDto = await response.ToResponseModel<SkillDto>();
            skillDto.Should().NotBeNull();
            skillDto!.Id.Value.Should().Be(_firstTestSkill.Id.Value);
            skillDto.Name.Should().Be(_firstTestSkill.Name);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenSkillDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST Tests

        [Fact]
        public async Task ShouldCreateSkill()
        {
            var request = new CreateSkillDto(
                Name: "New Test Skill"
            );

            var response = await Client.PostAsJsonAsync(BaseRoute, request);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var skillDto = await response.ToResponseModel<SkillDto>();
            skillDto.Should().NotBeNull();
            skillDto!.Name.Should().Be(request.Name);
            skillDto.Id.Should().NotBeNull();

            var dbSkill = await Context.Skills
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == skillDto.Id);

            dbSkill.Should().NotBeNull();
            dbSkill!.Name.Should().Be(request.Name);
        }

        #endregion

        #region PUT Tests

        [Fact]
        public async Task ShouldUpdateSkill()
        {
            var request = new UpdateSkillDto(
                Id: _firstTestSkill.Id.Value,
                Name: "Updated Skill Name"
            );

            var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestSkill.Id.Value}", request);
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var updatedSkill = await Context.Skills
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == _firstTestSkill.Id);

            updatedSkill.Should().NotBeNull();
            updatedSkill!.Name.Should().Be(request.Name);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenUpdatingNonExistentSkill()
        {
            var nonExistentId = Guid.NewGuid();

            var request = new UpdateSkillDto(
                Id: nonExistentId,
                Name: "Nonexistent"
            );

            var response = await Client.PutAsJsonAsync($"{BaseRoute}/{nonExistentId}", request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region DELETE Tests

        [Fact]
        public async Task ShouldDeleteSkill()
        {
            var response = await Client.DeleteAsync($"{BaseRoute}/{_secondTestSkill.Id.Value}");
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var skillExists = await Context.Skills
                .AnyAsync(s => s.Id == _secondTestSkill.Id);
            skillExists.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenDeletingNonExistentSkill()
        {
            var nonExistentId = Guid.NewGuid();
            var response = await Client.DeleteAsync($"{BaseRoute}/{nonExistentId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}