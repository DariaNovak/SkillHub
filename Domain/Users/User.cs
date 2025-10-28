using Domain.Roles.Role;
using Domain.UsersSkills;

namespace Domain.Users
{
    public class User
    {
        public UserId Id { get; set; } 
        public string Name { get; set; } 
        public string Email { get; set; } 
        public string PasswordHash { get; private set; } 
        public Guid RoleId { get; set; }
        public Role Role { get ; set; } 
        public DateTime JoinDate { get; private set; } 

        public ICollection<UserSkill> UserSkills { get; private set; }


        private User(
            UserId id,
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
                UserId.New(),
                name,
                email,
                passwordHash,
                roleId,
                DateTime.Now);
        }

        public void UpdateInfo(string name,
            string email,
            string passwordHash,
            Guid roleId,
            DateTime joinDate)
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            RoleId = roleId;
            JoinDate = joinDate;
        }
    }
}
