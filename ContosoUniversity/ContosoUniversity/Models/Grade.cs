using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Models
{
    public class Grade
    {
        public int GradeID { get; set; }

        [StringLength(1)]
        [DisplayFormat(NullDisplayText = "No grade")]
        [Required]
        public string  Letter { get; set; }
        
        [Range(0, 4)]
        public int Value { get; set; }
    }
}