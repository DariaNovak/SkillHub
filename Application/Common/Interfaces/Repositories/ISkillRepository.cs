using Domain.Skills;

namespace Application.Common.Interfaces.Repositories
{
    public interface ISkillRepository
    {
        Task<Skill> AddAsync(Skill entity, CancellationToken cancellationToken);
        Task UpdateAsync(Skill entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
