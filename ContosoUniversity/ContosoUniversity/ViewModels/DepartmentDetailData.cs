using ContosoUniversity.Models;
using System.Collections.Generic;

namespace ContosoUniversity.ViewModels
{
    public class DepartmentDetailData
    {
        public Department Department { get; set; }
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Coruses { get; set; }
    }
}