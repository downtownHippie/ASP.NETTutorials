using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class GPA
    {
        [Key]
        [ForeignKey("Student")]
        public int StudentID { get; set; }

        [DisplayFormat(NullDisplayText = "No GPA")]
        public double? Value { get; set; }

        public virtual Student Student { get; set; }
    }
}