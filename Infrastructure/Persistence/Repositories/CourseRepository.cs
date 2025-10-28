using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Settings;
using Domain.Courses;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourseRepository : ICourseRepository, ICourseQueries
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context, ApplicationSettings settings)
    {
        var connectionString = settings.ConnectionStrings.DefaultConnection;

        _context = context;
    }

    public async Task<Course> AddAsync(Course entity, CancellationToken cancellationToken)
    {
        await _context.Courses.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Option<IReadOnlyList<Course>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var courses = await _context.Courses
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return courses ?? Option<IReadOnlyList<Course>>.None;
    }

    public async Task<Option<Course?>> GetByIdAsync(CourseId id, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return course ?? Option<Course>.None;
    }

    public async Task UpdateAsync(Course entity, CancellationToken cancellationToken)
    {
        _context.Courses.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(CourseId id, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FindAsync(id, cancellationToken);

        if (course is not null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
