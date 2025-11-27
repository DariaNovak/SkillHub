using Domain.UsersSkills;
using Domain.Users;
using Domain.Skills;

namespace Tests.Data.UsersSkills
{
    public static class UserSkillData
    {
        public static UserSkill FirstUserSkill(UserId userId, SkillId skillId) =>
            UserSkill.New(userId, skillId, 4);

        public static UserSkill SecondUserSkill(UserId userId, SkillId skillId) =>
            UserSkill.New(userId, skillId, 3);

        public static UserSkill ThirdUserSkill(UserId userId, SkillId skillId) =>
            UserSkill.New(userId, skillId, 5);

        public static UserSkill WithCustomData(UserId userId, SkillId skillId, int proficiencyLevel) =>
            UserSkill.New(userId, skillId, proficiencyLevel);
    }
}
