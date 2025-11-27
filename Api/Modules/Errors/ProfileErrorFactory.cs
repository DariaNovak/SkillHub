using Application.Profiles.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors
{
    public static class ProfileErrorFactory
    {
        public static ObjectResult ToObjectResult(this ProfileException error)
        {
            return new ObjectResult(error.Message)
            {
                StatusCode = error switch
                {
                    ProfileAlreadyExistException => StatusCodes.Status409Conflict,
                    UserAlreadyHasProfileException => StatusCodes.Status409Conflict,
                    UnhandledProfileException => StatusCodes.Status500InternalServerError,
                    ProfileNotFoundException => StatusCodes.Status404NotFound,
                    _ => throw new NotImplementedException("Profile error handler does not implemented")
                }
            };
        }
    }
}
