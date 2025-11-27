using Application.Skills.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors
{
    public static class SkillErrorFactory
    {
        public static ObjectResult ToObjectResult(this SkillException error)
        {
            return new ObjectResult(error.Message)
            {
                StatusCode = error switch
                {
                    SkillAlreadyExistsException => StatusCodes.Status409Conflict,
                    UnhandledSkillException => StatusCodes.Status500InternalServerError,
                    SkillNotFoundException => StatusCodes.Status404NotFound,
                    _ => throw new NotImplementedException("Skill error handler does not implemented")
                }
            };
        }
    }
}
