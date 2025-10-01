using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Tasks.Commands.CreateTask;
using TaskManager.Application.Tasks.Commands.DeleteTask;
using TaskManager.Application.Tasks.Commands.UpdateTask;
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

    /// <summary>
    /// Cria uma nova tarefa
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetTask), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Obtém uma tarefa por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid id)
    {
        var query = new GetTaskQuery(id);
        var result = await _mediator.Send(query);
        
        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Lista todas as tarefas com filtros opcionais
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetTasks(
        [FromQuery] Guid? userId = null,
        [FromQuery] Domain.Enums.TaskStatus? status = null,
        [FromQuery] Domain.Enums.TaskPriority? priority = null,
        [FromQuery] string? searchTerm = null)
    {
        var query = new GetTasksQuery(userId, status, priority, searchTerm);
        var result = await _mediator.Send(query);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Atualiza uma tarefa existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID da URL não corresponde ao ID do comando");

        var result = await _mediator.Send(command);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Remove uma tarefa
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var command = new DeleteTaskCommand(id);
        var result = await _mediator.Send(command);
        
        if (result.IsFailure)
            return NotFound(result.Error);

        return NoContent();
    }
}
