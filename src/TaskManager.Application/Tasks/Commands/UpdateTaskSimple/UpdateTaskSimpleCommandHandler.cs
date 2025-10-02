using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Tasks.Commands.UpdateTaskSimple;

public class UpdateTaskSimpleCommandHandler : IRequestHandler<UpdateTaskSimpleCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskSimpleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateTaskSimpleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId, cancellationToken);
            if (task == null)
            {
                return Result.Failure("Tarefa não encontrada");
            }

            if (request.AssignedToUserId.HasValue)
            {
                var assignedUser = await _unitOfWork.Users.GetByIdAsync(request.AssignedToUserId.Value, cancellationToken);
                if (assignedUser == null)
                {
                    return Result.Failure("Usuário atribuído não encontrado");
                }
            }

            task.UpdateTitle(request.Title);
            task.UpdateDescription(request.Description);
            task.ChangeStatus(request.Status);
            task.ChangePriority(request.Priority);
            task.SetDueDate(request.DueDate);
            task.AssignToUser(request.AssignedToUserId);

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
