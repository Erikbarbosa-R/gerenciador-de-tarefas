using System;
using FluentValidation;

namespace TaskManager.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID da tarefa é obrigatório");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Prioridade deve ser Low (0), Medium (1) ou High (2)")
            .When(x => x.Priority.HasValue);

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Título não pode ter mais de 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Descrição não pode ter mais de 1000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.DueDate)
            .Must(dueDate => !dueDate.HasValue || dueDate.Value > DateTime.MinValue)
            .WithMessage("Data de vencimento inválida")
            .When(x => x.DueDate.HasValue);
    }
}
