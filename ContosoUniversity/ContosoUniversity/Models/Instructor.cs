using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public partial class Instructor : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}", ApplyFormatInEditMode=true)]
        [Display(Name="Hire Date")]
        public DateTime HireDate { get; set; }

        [ForeignKey("Department")]
        [Required(ErrorMessage = "Please select a department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}