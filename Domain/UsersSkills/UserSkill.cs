using Domain.Users;
using Domain.Skills;

namespace Domain.UsersSkills
{
    public class UserSkill
    {
        public Guid UserId { get; private set; }
        public Guid SkillId { get; private set; }

        public User User { get; private set; }
        public Skill Skill { get; private set; }

        public int ProficiencyLevel { get; private set; } // 1–5
        public DateTime AddedDate { get; private set; }

        private UserSkill(Guid userId, Guid skillId, int proficiencyLevel)
        {
            UserId = userId;
            SkillId = skillId;
            ProficiencyLevel = proficiencyLevel;
            AddedDate = DateTime.Now;
        }

        public static UserSkill New(User user, Skill skill, int proficiencyLevel = 3)
        {
            return new UserSkill(user.Id, skill.Id, proficiencyLevel);
        }
    }
}
