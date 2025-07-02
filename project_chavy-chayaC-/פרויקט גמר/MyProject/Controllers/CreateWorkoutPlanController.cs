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
        public async Task<ActionResult> GenerateWorkout([FromBody] GenerateWorkoutPlanRequestDto request)
        {
            Console.WriteLine("📥 נקלטה בקשה ל-GenerateWorkout");

            var user = await userRepository.GetByIdAsync(request.UserId);
            Console.WriteLine($"🔍 מתקבל userId: {request.UserId}, סוג אימון: {request.WorkoutType}");
            if (user == null)
            {
                Console.WriteLine("❌ משתמש לא נמצא");
                return NotFound("User not found");
            }

            var planner = new CreateWorkoutPlan(userRepository, videoRepository, planRepository);

            // עכשיו הפונקציה מחזירה רשימת DTO
            var videoDtos = await planner.GenerateWorkoutPlan(
                request.UserId,
                request.DesiredDuration,
                request.DifficultyLevel,
                request.WorkoutType,
                request.TargetAudience,
                request.IncludeWarmup,
                request.IncludeCooldown
            );

            // מחזירים את זה כמו שה-React מצפה
            return Ok(new { workoutPlanVideos = videoDtos });
        }

    }
}
