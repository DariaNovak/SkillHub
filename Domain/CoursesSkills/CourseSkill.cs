using System;
using Domain.Courses;
using Domain.Skills;

namespace Domain.CoursesSkills
{
    public class CourseSkill
    {
        public CourseId CourseId { get; private set; }
        public Guid SkillId { get; private set; }

        public Course Course { get; private set; }
        public Skill Skill { get; private set; }

        private CourseSkill(CourseId courseId, Guid skillId)
        {
            CourseId = courseId;
            SkillId = skillId;
        }

        public static CourseSkill New(Course course, Skill skill)
        {
            return new CourseSkill(course.Id, skill.Id);
        }
    }
}
