using Domain.Roles.Role;

namespace Application.Common.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> AddAsync(Role entity, CancellationToken cancellationToken);
        Task UpdateAsync(Role entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
