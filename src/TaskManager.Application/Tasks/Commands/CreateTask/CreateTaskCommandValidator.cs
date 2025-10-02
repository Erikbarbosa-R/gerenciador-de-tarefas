using System;
using FluentValidation;

namespace TaskManager.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é obrigatório")
            .MaximumLength(200).WithMessage("Título não pode ter mais de 200 caracteres");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é obrigatória")
            .MaximumLength(1000).WithMessage("Descrição não pode ter mais de 1000 caracteres");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("ID do usuário é obrigatório");

        RuleFor(x => x.DueDate)
            .Must(dueDate => !dueDate.HasValue || dueDate.Value > DateTime.MinValue)
            .WithMessage("Data de vencimento inválida")
            .When(x => x.DueDate.HasValue);
    }
}
