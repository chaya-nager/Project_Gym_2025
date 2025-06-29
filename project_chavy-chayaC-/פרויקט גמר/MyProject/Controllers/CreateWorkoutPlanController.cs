using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Repository.Interfaces;
using Service.Services;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateWorkoutPlanController : Controller
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<WorkoutVideo> videoRepository;
        private readonly IRepository<UserWorkoutPlan> planRepository;

        public CreateWorkoutPlanController(
            IRepository<User> userRepository,
            IRepository<WorkoutVideo> videoRepository,
            IRepository<UserWorkoutPlan> planRepository)
        {
            this.userRepository = userRepository;
            this.videoRepository = videoRepository;
            this.planRepository = planRepository;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<UserWorkoutPlan>> GenerateWorkout([FromBody] GenerateWorkoutPlanRequestDto request)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return NotFound("User not found");

            var planner = new CreateWorkoutPlan(userRepository, videoRepository, planRepository);
            var plan = await planner.GenerateWorkoutPlan(request.UserId, request.DesiredDuration, request.DifficultyLevel);

            return Ok(plan);
        }
    }

}
