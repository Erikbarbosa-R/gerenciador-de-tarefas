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

            task.UpdateTitle(request.Title);
            task.UpdateDescription(request.Description);
            task.ChangeStatus(request.Status);
            task.ChangePriority(request.Priority);
            task.SetDueDate(request.DueDate);

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
