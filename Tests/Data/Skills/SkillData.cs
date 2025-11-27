using Domain.Skills;

namespace Tests.Data.Skills
{
    public static class SkillData
    {
        public static Skill FirstSkill() =>
            Skill.New("C# Programming");

        public static Skill SecondSkill() =>
            Skill.New("ASP.NET Core");

        public static Skill ThirdSkill() =>
            Skill.New("Entity Framework");

        public static Skill WithCustomData(string name) =>
            Skill.New(name);
    }
}
