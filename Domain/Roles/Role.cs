using Domain.Users;

namespace Domain.Roles.Role
{
    public class Role
    {
        public Guid Id { get; private set; } 
        public string Name { get; private set; }

        public ICollection<User> Users { get; set; }

        private Role(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Role New(Guid id, string name)
        {
            return new Role(Guid.NewGuid(), name);
        }

        public void UpdateInfo(string name)
        {
            Name = name;
        }
    }
}
