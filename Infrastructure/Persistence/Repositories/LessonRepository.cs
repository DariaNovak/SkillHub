using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Common.Settings;
using Domain.Lessons;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class LessonRepository : ILessonRepository, ILessonQueries
{
    private readonly ApplicationDbContext _context;

    public LessonRepository(ApplicationDbContext context, ApplicationSettings settings)
    {
        var connectionString = settings.ConnectionStrings.DefaultConnection;
        _context = context;
    }

    public async Task<Lesson> AddAsync(Lesson entity, CancellationToken cancellationToken)
    {
        await _context.Lessons.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Option<IReadOnlyList<Lesson>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var lessons = await _context.Lessons
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return lessons.Any() ? Option<IReadOnlyList<Lesson>>.Some(lessons) : Option<IReadOnlyList<Lesson>>.None;
    }

    public async Task<Option<Lesson?>> GetByIdAsync(LessonId id, CancellationToken cancellationToken)
    {
        var lesson = await _context.Lessons
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        return lesson is not null ? Option<Lesson>.Some(lesson) : Option<Lesson>.None;
    }

    public async Task UpdateAsync(Lesson entity, CancellationToken cancellationToken)
    {
        _context.Lessons.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(LessonId id, CancellationToken cancellationToken)
    {
        var lesson = await _context.Lessons.FindAsync(id, cancellationToken);

        if (lesson is not null)
        {
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
