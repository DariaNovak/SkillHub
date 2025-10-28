

namespace Domain.Lessons
{
    public record LessonId(Guid Value)
    {
        public static LessonId Empty() => new(Guid.Empty);
        public static LessonId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
