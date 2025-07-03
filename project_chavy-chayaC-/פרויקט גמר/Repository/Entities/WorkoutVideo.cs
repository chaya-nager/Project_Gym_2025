using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class WorkoutVideo
    {
        [Key] // מפתח ראשי
        public int VideoId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Duration { get; set; } // משך הסרטון בדקות

    
        public string DifficultyLevel { get; set; } = "Beginner"; // Beginner, Intermediate, Advanced

        public string WorkoutType { get; set; } = string.Empty; // Cardio, Strength, Yoga...

        public string TargetAudience { get; set; } = string.Empty; // למשל: חולי לב 
        public string TargetAgeGroup { get; set; } = "לכולם";
        public string VideoUrl { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // מפתח זר - לכל סרטון יש מאמן שהעלה אותו
        [ForeignKey("User")]
        public int TrainerId { get; set; }
        public User? Trainer { get; set; }

        [ForeignKey("UserWorkoutPlan")]
        public int? UserWorkoutPlanId { get; set; }
        public UserWorkoutPlan? UserWorkoutPlan { get; set; }
    }
}
