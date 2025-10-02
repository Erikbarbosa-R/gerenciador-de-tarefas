using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Tasks.Commands.AssignTask;
using TaskManager.Application.Tasks.Commands.CreateTask;
using TaskManager.Application.Tasks.Commands.DeleteTask;
using TaskManager.Application.Tasks.Commands.UpdateTask;
using TaskManager.Application.Tasks.DTOs;
using TaskManager.Application.Tasks.Queries.GetTask;
using TaskManager.Application.Tasks.Queries.GetTasks;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetTask), new { id = result.Value }, result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid id)
    {
        var query = new GetTaskQuery(id);
        var result = await _mediator.Send(query);
        
        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(
        [FromQuery] Guid? userId = null,
        [FromQuery] Domain.Enums.TaskStatus? status = null,
        [FromQuery] Domain.Enums.TaskPriority? priority = null,
        [FromQuery] string? searchTerm = null)
    {
        var query = new GetTasksQuery(userId, status, priority, searchTerm);
        var tasks = await _mediator.Send(query);
        
        return Ok(tasks);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        await _mediator.Send(new DeleteTaskCommand(id));
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] PatchTaskDto dto)
    {
        var command = new UpdateTaskCommand(
            id,
            dto.Title,
            dto.Description,
            dto.Status,
            dto.Priority,
            dto.DueDate,
            dto.AssignedToUserId
        );

        var result = await _mediator.Send(command);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpPatch("{id}/assign")]
    public async Task<IActionResult> AssignTask(Guid id, [FromBody] AssignTaskCommand command)
    {
        var assignCommand = new AssignTaskCommand(id, command.AssignedToUserId);
        await _mediator.Send(assignCommand);
        return NoContent();
    }
}
