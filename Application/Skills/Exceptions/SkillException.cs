using Domain.Roles;
using Domain.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Skills.Exceptions
{
    public abstract class SkillException(SkillId skillId, string message, Exception? innerException = null)
       : Exception(message, innerException)
    {
        public SkillId SkillId { get; } = skillId;
    }

    public class SkillAlreadyExistsException(SkillId skillId)
        : SkillException(skillId, $"Lesson already exists under id {skillId}");

    public class SkillNotFoundException(SkillId skillId)
        : SkillException(skillId, $"Lesson not found under id {skillId}");

    public class UnhandledSkillException(SkillId skillId, Exception? innerException = null)
        : SkillException(skillId, "Unexpected error occurred", innerException);
}
