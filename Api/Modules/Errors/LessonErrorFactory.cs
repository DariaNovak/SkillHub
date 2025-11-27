using Application.Courses.Exceptions;
using Application.Lessons.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors
{
    public static class LessonErrorFactory
    {
        public static ObjectResult ToObjectResult(this LessonException error)
        {
            return new ObjectResult(error.Message)
            {
                StatusCode = error switch
                {
                    LessonAlreadyExistsException => StatusCodes.Status409Conflict,
                    UnhandledLessonException => StatusCodes.Status500InternalServerError,
                    LessonNotFoundException => StatusCodes.Status404NotFound,
                    _ => throw new NotImplementedException("Lesson error handler does not implemented")
                }
            };
        }
    }
}
