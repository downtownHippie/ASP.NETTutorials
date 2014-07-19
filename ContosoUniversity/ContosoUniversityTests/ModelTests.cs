using ContosoUniversity.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContosoUniversityTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void CourseModelTest()
        {
            Course course = new Course();
            Assert.AreEqual(0, course.Enrollments.Count);
            Assert.AreEqual(0, course.Instructors.Count);
        }

        [TestMethod]
        public void DepartmentModelTest()
        {
            Department department = new Department();
            Assert.AreEqual(0, department.Instructors.Count);
            Assert.AreEqual(0, department.Courses.Count);
        }

        [TestMethod]
        public void InstructorModelTest()
        {
            Instructor instructor = new Instructor();
            Assert.AreEqual(0, instructor.Courses.Count);
        }

        [TestMethod]
        public void StudentModelTest()
        {
            Student student = new Student();
            Assert.AreEqual(0, student.Enrollments.Count);
        }
    }
}
