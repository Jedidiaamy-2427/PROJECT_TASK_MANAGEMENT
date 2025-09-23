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
    public class ProjectsController(IProjectService service, IMapper mapper) : ControllerBase
    {
        private readonly IProjectService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<Project>> GetAll(CancellationToken ct)
        {
            var items = await _service.GetAllAsync(ct);
            return Ok(_mapper.Map<List<Project>>(items));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id, CancellationToken ct)
        {
            var project = await _service.GetByIdAsync(id, ct);
            return project is null ? NotFound() : Ok(_mapper.Map<ProjectDto>(project));
        }

        [HttpGet("by-user/{userId:int}")]
        public async Task<ActionResult<List<ProjectDto>>> GetByUserId(int userId, CancellationToken ct)
        {
            var items = await _service.GetByUserIdAsync(userId, ct);
            return Ok(_mapper.Map<List<ProjectDto>>(items));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(ProjectDto projectDto, CancellationToken ct)
        {   
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                              ?? User.FindFirst("sub")?.Value 
                              ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Utilisateur non authentifié.");
                
            var project = _mapper.Map<Project>(projectDto);
            project.UserId = int.Parse(userIdClaim);

            var created = await _service.CreateAsync(project, ct);
            var createdDto = _mapper.Map<ProjectDto>(created);  
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ProjectDto projectDto, CancellationToken ct)
        {
            var project = _mapper.Map<Project>(projectDto);
            project.Id = id;
            await _service.UpdateAsync(project, ct);
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


