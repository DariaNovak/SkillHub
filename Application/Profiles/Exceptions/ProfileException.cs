using Domain.Profiles;

namespace Application.Profiles.Exceptions;

public abstract class ProfileException : Exception
{
    public ProfileId Id { get; }

    protected ProfileException(ProfileId id, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        Id = id;
    }
}

public class ProfileNotFoundException : ProfileException
{
    public ProfileNotFoundException(ProfileId id)
        : base(id, $"Profile with id '{id}' not found")
    {
    }
}

public class ProfileAlreadyExistException : ProfileException
{
    public ProfileAlreadyExistException(ProfileId id)
        : base(id, $"Profile with id '{id}' already exists")
    {
    }
}

public class UnhandledProfileException : ProfileException
{
    public UnhandledProfileException(ProfileId id, Exception innerException)
        : base(id, $"An unhandled error occurred while processing profile '{id}'", innerException)
    {
    }
}

public class UserAlreadyHasProfileException : ProfileException
{
    public UserAlreadyHasProfileException(ProfileId id)
        : base(id, $"User already has a profile")
    {
    }
}
