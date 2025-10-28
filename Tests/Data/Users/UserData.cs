using Domain.Users;

namespace Tests.Data.Users
{
    public static class UserData
    {
        public static User FirstUser(Guid roleId) =>
            User.New(
                new UserId(Guid.NewGuid()),
                "Alice Johnson",
                "alice.johnson@example.com",
                "hashed_password_1",
                roleId,
                DateTime.Now
            );

        public static User SecondUser(Guid roleId) =>
            User.New(
                new UserId(Guid.NewGuid()),
                "Bob Smith",
                "bob.smith@example.com",
                "hashed_password_2",
                roleId,
                DateTime.Now
            );

        public static User ThirdUser(Guid roleId) =>
            User.New(
                new UserId(Guid.NewGuid()),
                "Charlie Brown",
                "charlie.brown@example.com",
                "hashed_password_3",
                roleId,
                DateTime.Now
            );

        public static User WithCustomData(
            string name,
            string email,
            string passwordHash,
            Guid roleId)
            => User.New(
                new UserId(Guid.NewGuid()),
                name,
                email,
                passwordHash,
                roleId,
                DateTime.Now
            );
    }
}
