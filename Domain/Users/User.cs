using Domain.Roles;
using Domain.Roles.Role;
using Domain.UsersSkills;
using Domain.Profiles;

namespace Domain.Users
{
    public class User
    {
        public UserId Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; private set; }
        public RoleId RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime JoinDate { get; private set; }

        public ICollection<UserSkill> UserSkills { get; set; }
        public Profile? Profile { get; set; }


        private User(
            UserId id,
            string name,
            string email,
            string passwordHash,
            RoleId roleId,
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
            RoleId roleId,
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
            RoleId roleId,
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