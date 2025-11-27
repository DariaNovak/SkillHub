using Application.Roles.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors
{
    public static class RoleErrorFactory
    {
        public static ObjectResult ToObjectResult(this RoleException error)
        {
            return new ObjectResult(error.Message)
            {
                StatusCode = error switch
                {
                    RoleAlreadyExistsException => StatusCodes.Status409Conflict,
                    UnhandledRoleException => StatusCodes.Status500InternalServerError,
                    RoleNotFoundException => StatusCodes.Status404NotFound,
                    _ => throw new NotImplementedException("Role error handler does not implemented")
                }
            };
        }
    }
}
