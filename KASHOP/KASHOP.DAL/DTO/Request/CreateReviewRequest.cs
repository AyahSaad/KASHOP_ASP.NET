using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO.Request
{
    public class CreateReviewRequest
    {
        [Required]
        [Range(1,5)]
        public int Rating { get; set; }
        [Required]
        [MinLength(5)]
        public string Comments { get; set; }
    }
}
