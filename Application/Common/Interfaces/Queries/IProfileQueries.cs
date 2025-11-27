using Domain.Profiles;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IProfileQueries
{
    Task<Option<Profile>> GetByIdAsync(ProfileId id, CancellationToken cancellationToken);
    Task<Option<Profile>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Profile>> GetAllAsync(CancellationToken cancellationToken);
}
