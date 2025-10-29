using Domain.Users;

namespace Domain.Roles.Role
{
    public class Role
    {
        public RoleId Id { get; private set; } 
        public string Name { get; private set; }

        public ICollection<User> Users { get; set; }

        private Role(RoleId id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Role New(string name)
        {
            return new Role(RoleId.New(), name);
        }

        public void UpdateInfo(string name)
        {
            Name = name;
        }
    }
}
