using Domain.Skills;

namespace Application.Common.Interfaces.Queries
{
    public interface ISkillQueries
    {
        Task<Skill?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Skill>> GetAllAsync(CancellationToken cancellationToken);
    }
}
