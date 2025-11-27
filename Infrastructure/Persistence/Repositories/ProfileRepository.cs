using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Profiles;
using Domain.Users;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ProfileRepository(ApplicationDbContext context) : IProfileRepository, IProfileQueries
{
    public async Task<Profile> AddAsync(Profile profile, CancellationToken cancellationToken)
    {
        await context.Profiles.AddAsync(profile, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return profile;
    }

    public async Task<Profile> UpdateAsync(Profile profile, CancellationToken cancellationToken)
    {
        context.Profiles.Update(profile);
        await context.SaveChangesAsync(cancellationToken);

        return profile;
    }

    public async Task<Profile> DeleteAsync(Profile profile, CancellationToken cancellationToken)
    {
        context.Profiles.Remove(profile);
        await context.SaveChangesAsync(cancellationToken);

        return profile;
    }

    public async Task<Option<Profile>> GetByIdAsync(ProfileId id, CancellationToken cancellationToken)
    {
        var entity = await context.Profiles
            .Include(p => p.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        return entity == null ? Option<Profile>.None : Option<Profile>.Some(entity);
    }

    public async Task<Option<Profile>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userIdValueObject = new UserId(userId);
        var entity = await context.Profiles
            .Include(p => p.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userIdValueObject, cancellationToken);

        return entity == null ? Option<Profile>.None : Option<Profile>.Some(entity);
    }

    public async Task<IReadOnlyList<Profile>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Profiles
            .Include(p => p.User)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
