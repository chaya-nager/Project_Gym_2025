using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    { 
        private IService<UserDto> _service;
        public UserController(IService<UserDto> _service)
        {
            this._service = _service;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<List<UserDto>> GetUsers()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(null);
            }
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task Post(UserDto value)
        {  
           await _service.AddItemAsync(value);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public  async Task<ActionResult<UserDto>> Put(int id, [FromForm] UserDto user)
        {
            if (id != user.UserId)
            {
                return BadRequest("User ID mismatch.");
            }
            var userUpdate =await _service.GetByIdAsync(id);
            if (userUpdate == null)
            {
                return NotFound();
            }
            await _service.UpdateItemAsync(id, user);
            return Ok(userUpdate);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user =await _service.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            } 
            await _service.DeleteItemAsync(id);
            return NoContent(); // מחזיר 204 בלי תוכן
        }
    }
}
