using Domain.CoursesSkills;
using Domain.Courses;
using Domain.Skills;

namespace Tests.Data.CoursesSkills
{
    public static class CourseSkillData
    {
        public static CourseSkill FirstCourseSkill(Course course, Skill skill) =>
            CourseSkill.New(course, skill);

        public static CourseSkill SecondCourseSkill(Course course, Skill skill) =>
            CourseSkill.New(course, skill);

        public static CourseSkill ThirdCourseSkill(Course course, Skill skill) =>
            CourseSkill.New(course, skill);
    }
}
