using Domain.Users;

namespace Application.Common.Interfaces.Queries
{
    public interface IUserQueries
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);
 
    }
}
