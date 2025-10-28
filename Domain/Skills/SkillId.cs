using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Skills
{
    public record SkillId(Guid Value)
    {
        public static SkillId Empty() => new(Guid.Empty);
        public static SkillId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
