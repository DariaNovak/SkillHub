using Domain.Roles.Role;
using Domain.UsersSkills;

namespace Domain.Users
{
    public class User
    {
        public Guid Id { get; private set; } 
        public string Name { get; set; } 
        public string Email { get; set; } 
        public string PasswordHash { get; private set; } 
        public Guid RoleId { get; private set; }
         public Role Role { get ; private set; } 
        public DateTime JoinDate { get; private set; } 

        public ICollection<UserSkill> UserSkills { get; private set; }


        private User(
            Guid id,
            string name,
            string email,
            string passwordHash,
            Guid roleId,
            DateTime joinDate)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            RoleId = roleId;
            JoinDate = joinDate;
        }
        public static User New(
            string name,
            string email,
            string passwordHash,
            Guid roleId,
            DateTime joinDate)
        {
            return new User(
              Guid.NewGuid(),
                name,
                email,
                passwordHash,
                roleId,
                DateTime.Now);
        }

        public void UpdateInfo(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
