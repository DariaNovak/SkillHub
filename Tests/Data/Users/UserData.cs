using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Data.Users
{
    public static class UserData
    {
        public static User FirstUser() =>
            User.New(
                new UserId(Guid.NewGuid()),
                "Alice Johnson",
                "alice.johnson@example.com",
                "hashed_password_1",
                Guid.NewGuid(), 
                DateTime.Now
            );

        public static User SecondUser() =>
            User.New(
                new UserId(Guid.NewGuid()),
                "Bob Smith",
                "bob.smith@example.com",
                "hashed_password_2",
                Guid.NewGuid(), 
                DateTime.Now
            );

        public static User ThirdUser() =>
            User.New(
                new UserId(Guid.NewGuid()),
                "Charlie Brown",
                "charlie.brown@example.com",
                "hashed_password_3",
                Guid.NewGuid(), 
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
