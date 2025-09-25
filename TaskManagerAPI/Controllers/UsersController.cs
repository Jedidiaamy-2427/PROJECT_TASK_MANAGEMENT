using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;
using TaskManagerAPI.Dtos;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController(IUserService service, IMapper mapper) : ControllerBase
    {
        private readonly IUserService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll(CancellationToken ct)
        {
            var items = await _service.GetAllAsync(ct);
            return Ok(_mapper.Map<List<User>>(items));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken ct)
        {
            var user = await _service.GetByIdAsync(id, ct);
            return user is null ? NotFound() : Ok(_mapper.Map<UserDto>(user));
        }

        [HttpGet("by-email")]
        public async Task<ActionResult<UserDto>> GetByEmail([FromQuery] string email, CancellationToken ct)
        {
            var user = await _service.GetByEmailAsync(email, ct);
            return user is null ? NotFound() : Ok(_mapper.Map<UserDto>(user));
        }

        [HttpGet("by-username")]
        public async Task<ActionResult<UserDto>> GetByUsername([FromQuery] string username, CancellationToken ct)
        {
            var user = await _service.GetByUsernameAsync(username, ct);
            return user is null ? NotFound() : Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(UserDto userDto, CancellationToken ct)
        {
            var user = _mapper.Map<User>(userDto);
            var created = await _service.CreateAsync(user, ct);
            var createdDto = _mapper.Map<UserDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UserDto userDto, CancellationToken ct)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = id;
            await _service.UpdateAsync(user, ct);
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


