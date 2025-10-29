using Domain.Skills;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface ISkillQueries
    {
        Task<Option<Skill>> GetByIdAsync(SkillId id, CancellationToken cancellationToken);
        Task<Option<IReadOnlyList<Skill>>> GetAllAsync(CancellationToken cancellationToken);
    }
}
