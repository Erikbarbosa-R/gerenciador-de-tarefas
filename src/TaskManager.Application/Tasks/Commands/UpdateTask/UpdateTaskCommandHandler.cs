using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.Id, cancellationToken);
            if (task == null)
            {
                return Result.Failure("Tarefa não encontrada");
            }

            // Atualizar apenas os campos fornecidos
            if (request.Title != null)
                task.UpdateTitle(request.Title);

            if (request.Description != null)
                task.UpdateDescription(request.Description);

            if (request.Status.HasValue)
                task.ChangeStatus(request.Status.Value);

            if (request.Priority.HasValue)
                task.ChangePriority(request.Priority.Value);

            if (request.DueDate.HasValue)
                task.SetDueDate(request.DueDate.Value);

            if (request.AssignedToUserId.HasValue)
            {
                var assignedUser = await _unitOfWork.Users.GetByIdAsync(request.AssignedToUserId.Value, cancellationToken);
                if (assignedUser == null && request.AssignedToUserId != Guid.Empty)
                {
                    return Result.Failure("Usuário atribuído não encontrado");
                }
                task.AssignToUser(request.AssignedToUserId);
            }

            await _unitOfWork.Tasks.UpdateAsync(task, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro ao atualizar tarefa: {ex.Message}");
        }
    }
}
