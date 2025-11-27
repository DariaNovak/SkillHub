using Domain.Profiles;

namespace Application.Common.Interfaces.Repositories;

public interface IProfileRepository
{
    Task<Profile> AddAsync(Profile profile, CancellationToken cancellationToken);
    Task<Profile> UpdateAsync(Profile profile, CancellationToken cancellationToken);
    Task<Profile> DeleteAsync(Profile profile, CancellationToken cancellationToken);
}
