using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.UsersSkills;
using Domain.Users;
using Domain.Skills;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserSkillRepository : IUserSkillRepository, IUserSkillQueries
{
    private readonly ApplicationDbContext _context;

    public UserSkillRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserSkill> AddAsync(UserSkill userSkill, CancellationToken cancellationToken = default)
    {
        await _context.UserSkills.AddAsync(userSkill, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return userSkill;
    }

    public async Task<UserSkill> UpdateAsync(UserSkill userSkill, CancellationToken cancellationToken = default)
    {
        var existing = await _context.UserSkills
            .FirstOrDefaultAsync(us => us.UserId == userSkill.UserId && us.SkillId == userSkill.SkillId, cancellationToken);

        if (existing is not null)
        {
            _context.Entry(existing).CurrentValues.SetValues(userSkill);
            await _context.SaveChangesAsync(cancellationToken);
            return userSkill;
        }

        throw new InvalidOperationException($"UserSkill with UserId '{userSkill.UserId}' and SkillId '{userSkill.SkillId}' not found.");
    }

    public async Task DeleteAsync(UserId userId, SkillId skillId, CancellationToken cancellationToken = default)
    {
        var userSkill = await _context.UserSkills
            .FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId, cancellationToken);

        if (userSkill is not null)
        {
            _context.UserSkills.Remove(userSkill);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Option<UserSkill>> GetByIdAsync(UserId userId, SkillId skillId, CancellationToken cancellationToken = default)
    {
        var userSkill = await _context.UserSkills
            .AsNoTracking()
            .Include(us => us.User)
            .Include(us => us.Skill)
            .FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId, cancellationToken);

        return userSkill is not null
            ? Option<UserSkill>.Some(userSkill)
            : Option<UserSkill>.None;
    }

    public async Task<List<UserSkill>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.UserSkills
            .AsNoTracking()
            .Include(us => us.User)
            .Include(us => us.Skill)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<UserSkill>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserSkills
            .AsNoTracking()
            .Include(us => us.User)
            .Include(us => us.Skill)
            .Where(us => us.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}
