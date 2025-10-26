using Domain.Users;
using Domain.Skills;

namespace Domain.UsersSkills
{
    public class UserSkill
    {
        public Guid Id { get; private set; }

        public UserId UserId { get; set; }
        public User User { get; private set; }

        public Guid SkillId { get; set; }
        public Skill Skill { get; private set; }

        public int ProficiencyLevel { get; private set; } // 1–5
        public DateTime AddedDate { get; private set; }

        private UserSkill(Guid id, UserId userId, Guid skillId, int proficiencyLevel, DateTime addedDate)
        {
            Id = id;
            UserId = userId;
            SkillId = skillId;
            ProficiencyLevel = proficiencyLevel;
            AddedDate = addedDate;
        }

        public static UserSkill New(UserId userId, Guid skillId, int proficiencyLevel = 3)
        {
            return new UserSkill(Guid.NewGuid(), userId, skillId, proficiencyLevel, DateTime.UtcNow);
        }
    }
}