using Domain.Roles;
using Domain.Users;

namespace Tests.Data.Users
{
    public static class UserData
    {
        public static User FirstUser(RoleId roleId) =>
            User.New(
                "Alice Johnson",
                "alice.johnson@example.com",
                "hashed_password_1",
                roleId,
                DateTime.Now
            );

        public static User SecondUser(RoleId roleId) =>
            User.New(
                "Bob Smith",
                "bob.smith@example.com",
                "hashed_password_2",
                roleId,
                DateTime.Now
            );

        public static User ThirdUser(RoleId roleId) =>
            User.New(
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
            RoleId roleId)
            => User.New(
                name,
                email,
                passwordHash,
                roleId,
                DateTime.Now
            );
    }
}
