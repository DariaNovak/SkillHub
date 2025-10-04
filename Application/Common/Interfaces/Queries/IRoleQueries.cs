using Domain.Roles.Role;

namespace Application.Common.Interfaces.Queries
{
    public interface IRoleQueries
    {
        Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken);
    }
}
