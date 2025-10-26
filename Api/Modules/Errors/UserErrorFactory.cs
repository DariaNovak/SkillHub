using Application.Users.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors
{
    public static class UserErrorFactory
    {
        public static ObjectResult ToObjectResult(this UserException error)
        {
            return new ObjectResult(error.Message)
            {
                StatusCode = error switch
                {
                    UserAlreadyExistException => StatusCodes.Status409Conflict,
                    UnhandledUserException => StatusCodes.Status500InternalServerError,
                    UserNotFoundException => StatusCodes.Status404NotFound,
                    _ => throw new NotImplementedException("User error handler does not implemented")
                }
            };
        }
    }
}
