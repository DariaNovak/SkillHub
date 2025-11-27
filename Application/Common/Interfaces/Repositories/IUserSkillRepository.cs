using Domain.UsersSkills;
using Domain.Users;
using Domain.Skills;

namespace Application.Common.Interfaces.Repositories;

public interface IUserSkillRepository
{
    Task<UserSkill> AddAsync(UserSkill userSkill, CancellationToken cancellationToken = default);
    Task<UserSkill> UpdateAsync(UserSkill userSkill, CancellationToken cancellationToken = default);
    Task DeleteAsync(UserId userId, SkillId skillId, CancellationToken cancellationToken = default);
}
