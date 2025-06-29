using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserWorkoutPlanController : ControllerBase
    {
        private readonly IService<UserWorkoutPlanDto> _service;

        public UserWorkoutPlanController(IService<UserWorkoutPlanDto> service)
        {
            _service = service;
        }
        // GET: api/<UserWorkoutPlanController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWorkoutPlanDto>>> GetUserWorkoutPlans()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<UserWorkoutPlanController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWorkoutPlanDto>> GetUserById(int id)
        {
            var userWorkoutPlan = await _service.GetByIdAsync(id);
            if (userWorkoutPlan == null)
            {
                return NotFound();
            }
            return userWorkoutPlan;
        }
        // POST api/<UserWorkoutPlanController>
        [HttpPost]
        public async Task Post(UserWorkoutPlanDto userWorkoutPlan)
        {
           await _service.AddItemAsync(userWorkoutPlan);
        }


        // PUT api/<UserWorkoutPlanController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserWorkoutPlanDto>> PutUserWorkoutPlan(int id, [FromBody] UserWorkoutPlanDto userWorkoutPlan)
        {
            if (id != userWorkoutPlan.Id)
            {
                return BadRequest("User ID mismatch.");
            }
            var userUpdate = await _service.GetByIdAsync(id);
            if (userUpdate == null)
            {
                return NotFound();
            }
            await _service.UpdateItemAsync(id, userWorkoutPlan);

            List<int> videoIds = userWorkoutPlan.VideoIds;
            foreach (int item in videoIds)
            {
                Console.WriteLine(item);
            }
            
            return Ok(userUpdate);
        }
        // DELETE api/<UserWorkoutPlanController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserWorkoutPlan(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _service.DeleteItemAsync(id);
            return NoContent(); 
        }

    }
}
