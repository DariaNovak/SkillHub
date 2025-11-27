using Application.CoursesSkills.Exceptions;

namespace Api.Modules.Errors;

public static class CourseSkillErrorFactory
{
    public static string GetErrorMessage(CourseSkillException exception)
    {
        return exception switch
        {
            CourseSkillNotFoundException => $"CourseSkill not found: {exception.Message}",
            CourseSkillAlreadyExistsException => $"CourseSkill already exists: {exception.Message}",
            UnhandledCourseSkillException => $"An error occurred: {exception.Message}",
            _ => "An unknown error occurred"
        };
    }
}
