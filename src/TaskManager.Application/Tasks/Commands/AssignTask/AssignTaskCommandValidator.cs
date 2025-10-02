using FluentValidation;

namespace TaskManager.Application.Tasks.Commands.AssignTask;

public class AssignTaskCommandValidator : AbstractValidator<AssignTaskCommand>
{
    public AssignTaskCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("ID da tarefa é obrigatório");

        RuleFor(x => x.AssignedToUserId)
            .NotEmpty()
            .When(x => x.AssignedToUserId.HasValue)
            .WithMessage("ID do usuário atribuído é obrigatório quando especificado");
    }
}
