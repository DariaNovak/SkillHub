using Domain.UsersSkills;
using Domain.Users;
using Domain.Skills;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IUserSkillQueries
{
    Task<Option<UserSkill>> GetByIdAsync(UserId userId, SkillId skillId, CancellationToken cancellationToken = default);
    Task<List<UserSkill>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<UserSkill>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
}
