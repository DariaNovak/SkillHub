using Domain.Users;

namespace Application.Users.Exceptions
{
   public abstract class UserException(UserId userId, string message, Exception? innerException = null)
    : Exception(message, innerException)
    {
        public UserId UserId { get; } = userId;
    }

    public class UserAlreadyExistException(UserId userId) : UserException(userId, $"User already exists under id {userId}");

    public class UserNotFoundException(UserId userId) : UserException(userId, $"User not found under id {userId}");

    public class UnhandledUserException(UserId userId, Exception? innerException = null)
        : UserException(userId, "Unexpected error occurred", innerException);
}
