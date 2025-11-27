using Domain.Lessons;
using Domain.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Roles.Exceptions
{
    public abstract class RoleException(RoleId roleId, string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
        public RoleId RoleId { get; } = roleId;
    }

    public class RoleAlreadyExistsException(RoleId roleId)
        : RoleException(roleId, $"Lesson already exists under id {roleId}");

    public class RoleNotFoundException(RoleId roleId)
        : RoleException(roleId, $"Lesson not found under id {roleId}");

    public class UnhandledRoleException(RoleId roleId, Exception? innerException = null)
        : RoleException(roleId, "Unexpected error occurred", innerException);
}
