using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Courses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CourseRepository(ApplicationDbContext context) : ICourseRepository, ICourseQueries
{
    public async Task<Course> AddAsync(Course entity, CancellationToken cancellationToken)
    {
        await context.Courses.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Courses.ToListAsync(cancellationToken);
    }

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Courses.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Course entity, CancellationToken cancellationToken)
    {
        context.Courses.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (course is not null)
        {
            context.Courses.Remove(course);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
