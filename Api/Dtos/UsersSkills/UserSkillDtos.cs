using Domain.UsersSkills;
using Domain.Users;
using Domain.Skills;

namespace Api.Dtos;

public record UserSkillDto(
    Guid Id,
    UserId UserId,
    SkillId SkillId,
    int ProficiencyLevel,
    DateTime AddedDate)
{
    public static UserSkillDto FromDomainModel(UserSkill userSkill)
        => new(
            userSkill.Id,
            userSkill.UserId,
            userSkill.SkillId,
            userSkill.ProficiencyLevel,
            userSkill.AddedDate);
}

public record CreateUserSkillDto(
    Guid UserId,
    Guid SkillId,
    int ProficiencyLevel = 3);

public record UpdateUserSkillDto(
    Guid UserId,
    Guid SkillId,
    int ProficiencyLevel);

public record DeleteUserSkillDto(
    Guid UserId,
    Guid SkillId);

public record GetUserSkillByIdDto(
    Guid UserId,
    Guid SkillId);

public record GetUserSkillsByUserIdDto(Guid UserId);
