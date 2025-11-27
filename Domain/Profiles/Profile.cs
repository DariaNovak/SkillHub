using Domain.Users;

namespace Domain.Profiles;

public class Profile
{
    public ProfileId Id { get; set; }
    public UserId UserId { get; set; }
    public User User { get; set; }
    public string Bio { get; set; }
    public string PhoneNumber { get; set; }
    public string Location { get; set; }
    public string Website { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Profile(
        ProfileId id,
        UserId userId,
        string bio,
        string phoneNumber,
        string location,
        string website,
        DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        Bio = bio;
        PhoneNumber = phoneNumber;
        Location = location;
        Website = website;
        CreatedAt = createdAt;
    }

    public static Profile New(
        UserId userId,
        string bio,
        string phoneNumber,
        string location,
        string website)
    {
        return new Profile(
            ProfileId.New(),
            userId,
            bio,
            phoneNumber,
            location,
            website,
            DateTime.UtcNow);
    }

    public void UpdateInfo(
        string bio,
        string phoneNumber,
        string location,
        string website)
    {
        Bio = bio;
        PhoneNumber = phoneNumber;
        Location = location;
        Website = website;
        UpdatedAt = DateTime.UtcNow;
    }
}
