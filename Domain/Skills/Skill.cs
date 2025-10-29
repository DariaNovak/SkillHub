using Domain.CoursesSkills;
using Domain.UsersSkills;

namespace Domain.Skills
{
    public class Skill
    {
        public SkillId Id { get; private set; }
        public string Name { get; private set; }

        public ICollection<UserSkill> UserSkills { get; private set; }
        public ICollection<CourseSkill> CourseSkills { get; private set; }

        private Skill(SkillId id, string name)
        {
            Id = id;
            Name = name;
            UserSkills = new List<UserSkill>();
        }

        public static Skill New(string name)
        {
            return new Skill(SkillId.New(), name);
        }

        public void UpdateInfo(string name)
        {
            Name = name;
        }
    }
}
