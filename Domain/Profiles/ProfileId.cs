namespace Domain.Profiles;

public record ProfileId(Guid Value)
{
    public static ProfileId New() => new(Guid.NewGuid());
    public static ProfileId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}
