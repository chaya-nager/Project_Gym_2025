using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class User
    {
        [Key] // מפתח ראשי
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public string UserType { get; set; } = "User"; // "User" או "Trainer"

        public string? HealthConditions { get; set; }

        // קשר 1 לרבים - משתמש יכול להעלות סרטונים אם הוא מאמן
        public ICollection<WorkoutVideo>? WorkoutVideos { get; set; }
        public UserWorkoutPlan UserWorkoutPlan { get; set; }

    }
}
