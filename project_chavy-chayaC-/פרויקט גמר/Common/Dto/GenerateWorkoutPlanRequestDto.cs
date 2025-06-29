using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class GenerateWorkoutPlanRequestDto
    {
        public int UserId { get; set; }
        public int DesiredDuration { get; set; }
        public string DifficultyLevel { get; set; }
    }
}
