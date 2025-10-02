using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Common.Models;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tasks.Commands.AssignTask;

public class AssignTaskCommandHandler : IRequestHandler<AssignTaskCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public AssignTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FindAsync(new object[] { request.TaskId }, cancellationToken);

        if (task == null)
        {
            return Result.Failure("Tarefa não encontrada");
        }

        if (request.AssignedToUserId.HasValue)
        {
            var assignedUser = await _context.Users.FindAsync(new object[] { request.AssignedToUserId.Value }, cancellationToken);
            if (assignedUser == null)
            {
                return Result.Failure("Usuário atribuído não encontrado");
            }
        }

        task.AssignToUser(request.AssignedToUserId);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
