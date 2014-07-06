using ContosoUniversity.Models;
using System.Collections.Generic;

namespace ContosoUniversity.ViewModels
{
    public class InstructorIndexData
    {
        public Instructor instructor { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}