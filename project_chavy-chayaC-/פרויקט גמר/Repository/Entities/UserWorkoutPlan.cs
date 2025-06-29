
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class UserWorkoutPlan
    {
        [Key] // מפתח ראשי
        public int Id { get; set; }

        // קשר ל-User
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        //// קשר ל-WorkoutVideo
        [ForeignKey("WorkoutVideo")]
        //public int VideoId { get; set; }
        //public WorkoutVideo WorkoutVideo { get; set; }
        public List<WorkoutVideo> WorkoutPlanVideos { get; set; } = new List<WorkoutVideo>();
    }
}
