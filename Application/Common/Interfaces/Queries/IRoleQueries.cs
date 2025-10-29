using Domain.Roles;
using Domain.Roles.Role;
using LanguageExt;

namespace Application.Common.Interfaces.Queries
{
    public interface IRoleQueries
    {
        Task<Option<Role?>> GetByIdAsync(RoleId id, CancellationToken cancellationToken);
        Task<Option<IReadOnlyList<Role>>> GetAllAsync(CancellationToken cancellationToken);
    }
}
