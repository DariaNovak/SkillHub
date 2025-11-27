using Api.Dtos;
using Domain.Profiles;
using Domain.Roles.Role;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Tests.Common;
using Tests.Data.Profiles;
using Tests.Data.Users;

namespace Tests.Api;

public class ProfileControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "http://localhost:5078/profiles";

    private Role _testRole = null!;
    private User _firstTestUser = null!;
    private User _secondTestUser = null!;
    private User _thirdTestUser = null!;
    private Profile _firstTestProfile = null!;
    private Profile _secondTestProfile = null!;

    public ProfileControllerTests(IntegrationTestWebFactory factory) : base(factory) { }

    public async Task InitializeAsync()
    {
        _testRole = Role.New("user");
        await Context.Roles.AddAsync(_testRole);
        await SaveChangesAsync();

        _firstTestUser = UserData.FirstUser(_testRole.Id);
        _secondTestUser = UserData.SecondUser(_testRole.Id);
        _thirdTestUser = UserData.ThirdUser(_testRole.Id);
        
        await Context.Users.AddAsync(_firstTestUser);
        await Context.Users.AddAsync(_secondTestUser);
        await Context.Users.AddAsync(_thirdTestUser);
        await SaveChangesAsync();

        _firstTestProfile = ProfileData.FirstProfile(_firstTestUser.Id);
        _secondTestProfile = ProfileData.SecondProfile(_secondTestUser.Id);
        
        await Context.Profiles.AddAsync(_firstTestProfile);
        await Context.Profiles.AddAsync(_secondTestProfile);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Profiles.RemoveRange(Context.Profiles);
        Context.Users.RemoveRange(Context.Users);
        Context.Roles.RemoveRange(Context.Roles);
        await SaveChangesAsync();
    }

    #region GET Tests

    [Fact]
    public async Task ShouldGetAllProfiles()
    {
        // Arrange
        // Test data is set up in InitializeAsync

        // Act
        var response = await Client.GetAsync(BaseRoute);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var profiles = await response.ToResponseModel<List<ProfileDto>>();
        profiles.Should().NotBeNull();
        profiles!.Count.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task ShouldGetProfileById()
    {
        // Arrange
        // Test data is set up in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_firstTestProfile.Id.Value}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var profileDto = await response.ToResponseModel<ProfileDto>();
        profileDto.Should().NotBeNull();
        profileDto!.Id.Value.Should().Be(_firstTestProfile.Id.Value);
        profileDto.Bio.Should().Be(_firstTestProfile.Bio);
        profileDto.PhoneNumber.Should().Be(_firstTestProfile.PhoneNumber);
        profileDto.Location.Should().Be(_firstTestProfile.Location);
        profileDto.Website.Should().Be(_firstTestProfile.Website);
    }

    [Fact]
    public async Task ShouldGetProfileByUserId()
    {
        // Arrange
        // Test data is set up in InitializeAsync

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/user/{_firstTestUser.Id.Value}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var profileDto = await response.ToResponseModel<ProfileDto>();
        profileDto.Should().NotBeNull();
        profileDto!.UserId.Value.Should().Be(_firstTestUser.Id.Value);
        profileDto.Bio.Should().Be(_firstTestProfile.Bio);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenProfileDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region POST Tests

    [Fact]
    public async Task ShouldCreateProfile()
    {
        // Arrange
        var request = new CreateProfileDto(
            UserId: _thirdTestUser.Id.Value,
            Bio: "New test profile bio",
            PhoneNumber: "+380991234567",
            Location: "Kharkiv, Ukraine",
            Website: "https://newprofile.example.com"
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var profileDto = await response.ToResponseModel<ProfileDto>();
        profileDto.Should().NotBeNull();
        profileDto!.Bio.Should().Be(request.Bio);
        profileDto.PhoneNumber.Should().Be(request.PhoneNumber);
        profileDto.Location.Should().Be(request.Location);
        profileDto.Website.Should().Be(request.Website);
        profileDto.UserId.Value.Should().Be(_thirdTestUser.Id.Value);

        var dbProfile = await Context.Profiles
            .FirstOrDefaultAsync(p => p.Id == profileDto.Id);

        dbProfile.Should().NotBeNull();
        dbProfile!.Bio.Should().Be(request.Bio);
        dbProfile.UserId.Should().Be(_thirdTestUser.Id);
    }

    [Fact]
    public async Task ShouldNotCreateProfileWhenUserAlreadyHasOne()
    {
        // Arrange
        var request = new CreateProfileDto(
            UserId: _firstTestUser.Id.Value,
            Bio: "Duplicate profile",
            PhoneNumber: "+380991234567",
            Location: "Test City",
            Website: "https://test.com"
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region PUT Tests

    [Fact]
    public async Task ShouldUpdateProfile()
    {
        // Arrange
        var request = new UpdateProfileDto(
            Id: _firstTestProfile.Id.Value,
            Bio: "Updated bio information",
            PhoneNumber: "+380501111111",
            Location: "Kyiv, Ukraine (Updated)",
            Website: "https://updated.example.com"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstTestProfile.Id.Value}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedProfile = await Context.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == _firstTestProfile.Id);

        updatedProfile.Should().NotBeNull();
        updatedProfile!.Bio.Should().Be(request.Bio);
        updatedProfile.PhoneNumber.Should().Be(request.PhoneNumber);
        updatedProfile.Location.Should().Be(request.Location);
        updatedProfile.Website.Should().Be(request.Website);
        updatedProfile.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenUpdatingNonExistentProfile()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var request = new UpdateProfileDto(
            Id: nonExistentId,
            Bio: "Nonexistent",
            PhoneNumber: "+380000000000",
            Location: "Nowhere",
            Website: "https://nowhere.com"
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{nonExistentId}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region DELETE Tests

    [Fact]
    public async Task ShouldDeleteProfile()
    {
        // Arrange
        // Test data is set up in InitializeAsync

        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{_secondTestProfile.Id.Value}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var profileExists = await Context.Profiles
            .AnyAsync(p => p.Id == _secondTestProfile.Id);
        profileExists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDeletingNonExistentProfile()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}
