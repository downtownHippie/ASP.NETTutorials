using ContosoUniversity.Models;
using System.Collections.Generic;

namespace ContosoUniversity.ViewModels
{
    public class InstructorDetailData
    {
        public Instructor Instructor { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}