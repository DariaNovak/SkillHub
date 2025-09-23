using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Roles.Role;

namespace Domain.Users
{
    public class User
    {
  

        public Guid Id { get; private set; } 
        public string Name { get; set; } 
        public string Email { get; set; } 
        public string PasswordHash { get; private set; } 
        public Role Role { get ; private set; } 
        public DateTime JoinDate { get; private set; } 


        public User(
            Guid id,
            string name,
            string email,
            string passwordHash,
            Role role,
            DateTime joinDate)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            JoinDate = joinDate;
        }
        public static User New(
            Guid id,
            string name,
            string email,
            string passwordHash,
            Role role,
            DateTime joinDate)
        {
            return new User(
              Guid.NewGuid(),
                name,
                email,
                passwordHash,
                role,
                DateTime.Now);
        }

        public void UpdateInfo(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
