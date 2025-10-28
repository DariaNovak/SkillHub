using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UsersSkills
{
    public record UserSkillId(Guid Value)
    {
        public static UserSkillId Empty() => new(Guid.Empty);
        public static UserSkillId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
