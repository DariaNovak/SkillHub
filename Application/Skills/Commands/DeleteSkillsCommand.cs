using Application.Common.Interfaces.Repositories;
using MediatR;

namespace Application.Skills.Commands;

public record DeleteSkillsCommand(Guid SkillId) : IRequest;

public class DeleteSkillsCommandHandler(
    ISkillRepository skillRepository) : IRequestHandler<DeleteSkillsCommand>
{
    public async Task Handle(DeleteSkillsCommand request, CancellationToken cancellationToken)
    {
        await skillRepository.DeleteAsync(request.SkillId, cancellationToken);
    }
}
