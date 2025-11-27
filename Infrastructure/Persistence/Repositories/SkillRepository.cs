using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Settings;
using Domain.Roles;
using Domain.Skills;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SkillRepository : ISkillRepository, ISkillQueries
{
    private readonly ApplicationDbContext _context;

    public SkillRepository(ApplicationDbContext context, ApplicationSettings settings)
    {
        var connectionString = settings.ConnectionStrings.DefaultConnection;

        _context = context;
    }

    public async Task<Skill> AddAsync(Skill entity, CancellationToken cancellationToken)
    {
        await _context.Skills.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Option<IReadOnlyList<Skill>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var skills = await _context.Skills
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return skills.Count > 0
            ? Option<IReadOnlyList<Skill>>.Some(skills)
            : Option<IReadOnlyList<Skill>>.None;
    }

    public async Task<Option<Skill>> GetByIdAsync(SkillId id, CancellationToken cancellationToken)
    {
        var skill = await _context.Skills
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return skill is not null
            ? Option<Skill>.Some(skill)
            : Option<Skill>.None;
    }

    public async Task UpdateAsync(Skill entity, CancellationToken cancellationToken)
    {
        _context.Skills.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var skillId = new SkillId(id);
        var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == skillId, cancellationToken);

        if (skill is not null)
        {
            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}