using Domain.Users;
using LanguageExt;
using LanguageExt.TypeClasses;

namespace Application.Common.Interfaces.Queries
{
    public interface IUserQueries
    {
        Task<Option<User?>> GetByIdAsync(UserId id, CancellationToken cancellationToken);
        Task<Option<IReadOnlyList<User>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Option<User>> GetByEmailAsync(string email, CancellationToken cancellationToken);

    }
}
