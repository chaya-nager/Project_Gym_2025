
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class UserWorkoutPlanDto
    {
     
        public int Id { get; set; }

        public int UserId { get; set; }

        public List<int> VideoIds { get; set; } = new List<int>();
    }

}

