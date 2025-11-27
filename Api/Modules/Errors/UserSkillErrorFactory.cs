using Application.UsersSkills.Exceptions;

namespace Api.Modules.Errors;

public static class UserSkillErrorFactory
{
    public static string GetErrorMessage(UserSkillException exception)
    {
        return exception switch
        {
            UserSkillNotFoundException => $"UserSkill not found: {exception.Message}",
            UserSkillAlreadyExistsException => $"UserSkill already exists: {exception.Message}",
            UnhandledUserSkillException => $"An error occurred: {exception.Message}",
            _ => "An unknown error occurred"
        };
    }
}
