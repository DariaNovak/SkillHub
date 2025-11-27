using Domain.Roles.Role;

namespace Tests.Data.Roles
{
    public static class RoleData
    {
        public static Role FirstRole() =>
            Role.New("Admin");

        public static Role SecondRole() =>
            Role.New("User");

        public static Role ThirdRole() =>
            Role.New("Moderator");

        public static Role WithCustomData(string name) =>
            Role.New(name);
    }
}