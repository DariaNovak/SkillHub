using Domain.UsersSkills;

namespace Domain.Skills
{
    public class Skill
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public ICollection<UserSkill> UserSkills { get; private set; }

        private Skill(Guid id, string name)
        {
            Id = id;
            Name = name;
            UserSkills = new List<UserSkill>();
        }

        public static Skill New(string name)
        {
            return new Skill(Guid.NewGuid(), name);
        }
    }
}
