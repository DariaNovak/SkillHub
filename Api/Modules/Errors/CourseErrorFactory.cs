using Application.Courses.Exceptions;
using Application.Users.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors
{
    public static class CourseErrorFactory
    {
        public static ObjectResult ToObjectResult(this CourseException error)
        {
            return new ObjectResult(error.Message)
            {
                StatusCode = error switch
                {
                    CourseAlreadyExistsException => StatusCodes.Status409Conflict,
                    UnhandledCourseException => StatusCodes.Status500InternalServerError,
                    CourseNotFoundException => StatusCodes.Status404NotFound,
                    _ => throw new NotImplementedException("Course error handler does not implemented")
                }
            };
        }
    }
}
