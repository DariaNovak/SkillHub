using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Lessons;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class LessonRepository(ApplicationDbContext context) : ILessonRepository, ILessonQueries
{
    public async Task<Lesson> AddAsync(Lesson entity, CancellationToken cancellationToken)
    {
        await context.Lessons.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<IReadOnlyList<Lesson>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Lessons.ToListAsync(cancellationToken);
    }

    public async Task<Lesson?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Lessons.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Lesson entity, CancellationToken cancellationToken)
    {
        context.Lessons.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var lesson = await context.Lessons.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        if (lesson is not null)
        {
            context.Lessons.Remove(lesson);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
