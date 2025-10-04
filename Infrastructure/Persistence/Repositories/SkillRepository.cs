using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Skills;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SkillRepository(ApplicationDbContext context) : ISkillRepository, ISkillQueries
{
    public async Task<Skill> AddAsync(Skill entity, CancellationToken cancellationToken)
    {
        await context.Skills.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<IReadOnlyList<Skill>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Skills.ToListAsync(cancellationToken);
    }

    public async Task<Skill?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Skills.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Skill entity, CancellationToken cancellationToken)
    {
        context.Skills.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var skill = await context.Skills.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (skill is not null)
        {
            context.Skills.Remove(skill);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
