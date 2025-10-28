using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CoursesSkills
{
    public record CourseSkillId(Guid Value)
    {
        public static CourseSkillId Empty() => new(Guid.Empty);
        public static CourseSkillId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
