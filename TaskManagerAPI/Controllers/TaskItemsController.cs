using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TaskManagerAPI.Dtos;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskItemsController(ITaskItemService service, IMapper mapper) : ControllerBase
    {
        private readonly ITaskItemService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<List<TaskItemDto>>> GetAll(CancellationToken ct)
        {
            var items = await _service.GetAllAsync(ct);
            return Ok(_mapper.Map<List<TaskItemDto>>(items));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskItemDto>> GetById(int id, CancellationToken ct)
        {
            var task = await _service.GetByIdAsync(id, ct);
            return task is null ? NotFound() : Ok(_mapper.Map<TaskItemDto>(task));
        }

        [HttpGet("by-project/{projectId:int}")]
        public async Task<ActionResult<List<TaskItemDto>>> GetByProjectId(int projectId, CancellationToken ct)
        {
            var items = await _service.GetByProjectIdAsync(projectId, ct);
            return Ok(_mapper.Map<List<TaskItemDto>>(items));
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemDto>> Create(TaskItemDto taskDto, CancellationToken ct)
        {
            var task = _mapper.Map<TaskItem>(taskDto);
            var created = await _service.CreateAsync(task, ct);
            var createdDto = _mapper.Map<TaskItemDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TaskItemDto taskDto, CancellationToken ct)
        {
            var task = _mapper.Map<TaskItem>(taskDto);
            task.Id = id;
            await _service.UpdateAsync(task, ct);
            return NoContent();
        }

        [HttpPost("{id:int}/toggle-complete")]
        public async Task<IActionResult> ToggleComplete(int id, CancellationToken ct)
        {
            await _service.ToggleCompleteAsync(id, ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _service.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}


