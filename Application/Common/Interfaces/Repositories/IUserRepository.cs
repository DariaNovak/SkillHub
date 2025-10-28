using Domain.Users;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User entity, CancellationToken cancellationToken);
        Task UpdateAsync(User entity, CancellationToken cancellationToken);
        Task DeleteAsync(UserId id, CancellationToken cancellationToken);
        
    }
}