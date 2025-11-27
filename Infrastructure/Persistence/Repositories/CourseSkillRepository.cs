using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.CoursesSkills;
using Domain.Courses;
using Domain.Skills;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourseSkillRepository : ICourseSkillRepository, ICourseSkillQueries
{
    private readonly ApplicationDbContext _context;

    public CourseSkillRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseSkill> AddAsync(CourseSkill courseSkill, CancellationToken cancellationToken = default)
    {
        await _context.CourseSkills.AddAsync(courseSkill, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return courseSkill;
    }

    public async Task DeleteAsync(CourseId courseId, SkillId skillId, CancellationToken cancellationToken = default)
    {
        var courseSkill = await _context.CourseSkills
            .FirstOrDefaultAsync(cs => cs.CourseId == courseId && cs.SkillId == skillId, cancellationToken);

        if (courseSkill is not null)
        {
            _context.CourseSkills.Remove(courseSkill);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Option<CourseSkill>> GetByIdAsync(CourseId courseId, SkillId skillId, CancellationToken cancellationToken = default)
    {
        var courseSkill = await _context.CourseSkills
            .AsNoTracking()
            .Include(cs => cs.Course)
            .Include(cs => cs.Skill)
            .FirstOrDefaultAsync(cs => cs.CourseId == courseId && cs.SkillId == skillId, cancellationToken);

        return courseSkill is not null
            ? Option<CourseSkill>.Some(courseSkill)
            : Option<CourseSkill>.None;
    }

    public async Task<List<CourseSkill>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CourseSkills
            .AsNoTracking()
            .Include(cs => cs.Course)
            .Include(cs => cs.Skill)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CourseSkill>> GetByCourseIdAsync(CourseId courseId, CancellationToken cancellationToken = default)
    {
        return await _context.CourseSkills
            .AsNoTracking()
            .Include(cs => cs.Course)
            .Include(cs => cs.Skill)
            .Where(cs => cs.CourseId == courseId)
            .ToListAsync(cancellationToken);
    }
}
