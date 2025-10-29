using Domain.Skills;

namespace Api.Dtos;

public record SkillDto(
    SkillId Id,
    string Name)
{
    public static SkillDto FromDomainModel(Skill skill)
        => new(
            skill.Id,
            skill.Name);
}

public record CreateSkillDto(
    string Name);

public record UpdateSkillDto(
    Guid Id,
    string Name);

public record DeleteSkillDto(Guid Id);

public record GetSkillByIdDto(Guid Id);
