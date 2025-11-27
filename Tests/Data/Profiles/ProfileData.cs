using Domain.Profiles;
using Domain.Users;

namespace Tests.Data.Profiles;

public static class ProfileData
{
    public static Profile FirstProfile(UserId userId)
    {
        return Profile.New(
            userId,
            "Experienced software engineer with passion for clean code",
            "+380501234567",
            "Kyiv, Ukraine",
            "https://example.com");
    }

    public static Profile SecondProfile(UserId userId)
    {
        return Profile.New(
            userId,
            "Full-stack developer and tech enthusiast",
            "+380671234567",
            "Lviv, Ukraine",
            "https://portfolio.example.com");
    }

    public static Profile ThirdProfile(UserId userId)
    {
        return Profile.New(
            userId,
            "DevOps engineer specializing in cloud infrastructure",
            "+380931234567",
            "Odesa, Ukraine",
            "https://blog.example.com");
    }
}
