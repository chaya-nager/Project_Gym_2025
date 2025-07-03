using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutVideoController : ControllerBase
    {
        private readonly IService<WorkoutVideoDto> _service;
        private readonly IWorkoutVideoService<WorkoutVideoDto> _workoutVideoService;
        public WorkoutVideoController(IService<WorkoutVideoDto> service, IWorkoutVideoService<WorkoutVideoDto> workoutVideoService)
        {
            _service = service;       
            _workoutVideoService= workoutVideoService;
        }
        // GET: api/<WorkoutVideoController>
        [HttpGet]
        public async Task<ActionResult<List<WorkoutVideoDto>>> GetWorkoutVideo()
        {
            return  Ok(await _service.GetAllAsync());
        }
        // GET api/<WorkoutVideoController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutVideoDto>> GetWorkoutVideoById(int id)
        {
            var WorkoutVideo =await _service.GetByIdAsync(id);
            if (WorkoutVideo == null)
            {
                return NotFound();
            }
            return Ok(WorkoutVideo);
        }
        [HttpGet("by-trainer/{trainerId}")]
        public async Task<IActionResult> GetVideosByTrainer(int trainerId)
        {
            var videos = await _workoutVideoService.GetVideosByTrainerId(trainerId);

            return Ok(videos);
        }
        // POST api/<WorkoutVideoController>
        [HttpPost]
        [Authorize (Roles= "Trainer")]
        public async Task<IActionResult> Post([FromForm] WorkoutVideoDto workoutVideo)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(workoutVideo.fileVideo.FileName);
            var videosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos");
            Directory.CreateDirectory(videosPath);
            var filePath = Path.Combine(videosPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await workoutVideo.fileVideo.CopyToAsync(stream);
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            workoutVideo.VideoUrl = $"{baseUrl}/Videos/{fileName}";

            await _service.AddItemAsync(workoutVideo);
            return Ok();
        }
        // PUT api/<WorkoutVideoController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] WorkoutVideoDto workoutVideo)
        {
            if (id != workoutVideo.VideoId)
            {
                return BadRequest("User ID mismatch.");
            }
            var workoutVideoUpdate = await _service.GetByIdAsync(id);
            if (workoutVideoUpdate == null)
            {
                return NotFound();
            }
            if (workoutVideo.fileVideo != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(workoutVideo.fileVideo.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await workoutVideo.fileVideo.CopyToAsync(stream);
                }
                workoutVideo.VideoUrl = $"{Request.Scheme}://{Request.Host}/Videos/{fileName}";
            }
            else
            {
                workoutVideo.VideoUrl = workoutVideoUpdate.VideoUrl; // שמור את הקישור הקיים אם לא שונה
            }
            await _service.UpdateItemAsync(id, workoutVideo);
            return NoContent();
        }
        // DELETE api/<WorkoutVideoController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutVideo(int id)
        {
            var workoutVideo = await _service.GetByIdAsync(id);
            if (workoutVideo == null)
                return NotFound("הסרטון לא נמצא.");

            // נסה למחוק את הקובץ הפיזי אם יש כתובת URL
            if (!string.IsNullOrWhiteSpace(workoutVideo.VideoUrl))
            {
                try
                {
                    var uri = new Uri(workoutVideo.VideoUrl);
                    var fileName = Path.GetFileName(uri.LocalPath);
                    var videoFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos");
                    var filePath = Path.Combine(videoFolder, fileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    // אפשר לרשום ליומן אבל לא לחסום את המחיקה מה־DB
                    Console.WriteLine($"⚠ שגיאה במחיקת קובץ פיזי: {ex.Message}");
                }
            }

            await _service.DeleteItemAsync(id);
            return Ok("הסרטון נמחק בהצלחה.");
        }

        private async Task UploadVideo(IFormFile file)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Videos\\", file.FileName);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
