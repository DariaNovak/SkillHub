using Domain.Users;
using Domain.Skills;

namespace Application.UsersSkills.Exceptions;

public abstract class UserSkillException(UserId userId, SkillId skillId, string message)
    : Exception(message)
{
    public UserId UserId { get; } = userId;
    public SkillId SkillId { get; } = skillId;
}

public class UserSkillNotFoundException(UserId userId, SkillId skillId)
    : UserSkillException(userId, skillId, $"UserSkill with UserId '{userId}' and SkillId '{skillId}' not found");

public class UserSkillAlreadyExistsException(UserId userId, SkillId skillId)
    : UserSkillException(userId, skillId, $"UserSkill with UserId '{userId}' and SkillId '{skillId}' already exists");

public class UnhandledUserSkillException(UserId userId, SkillId skillId, Exception innerException)
    : UserSkillException(userId, skillId, $"An unhandled error occurred for UserSkill with UserId '{userId}' and SkillId '{skillId}'")
{
    public Exception InnerException { get; } = innerException;
}
