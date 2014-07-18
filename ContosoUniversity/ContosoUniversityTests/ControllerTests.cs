using System;
using ContosoUniversity.DAL;
using ContosoUniversity.Controllers;
using ContosoUniversity.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Entity;

namespace ContosoUniversityTests
{
    [TestClass]
    public class ControllerTests
    {
        private SchoolContext db;
        private ControllerTestObjects objects;

        [TestInitialize()]
        public void TestInitialize()
        {
            db = new SchoolContext();
            objects = new ControllerTestObjects(3);

            DatabaseReset();

            DepartmentController departmentCreateController = new DepartmentController();
            Task<ActionResult> task = departmentCreateController.Create(objects.department);
            task.Wait();

            // if a department is not correctly created might as well stop the test
            Assert.AreEqual(TaskStatus.RanToCompletion, task.Status,
                "department did not make, task did not complete correctly");
            DoesDepartmentExist(true);

            objects.SetDepartmentID(objects.department.DepartmentID);

            for (int i = 0; i < objects.NumberOfDerivativeObjects; i++)
            {
                CourseController courseController = new CourseController();
                ActionResult courseControllerCreateResult = courseController.Create(objects.Courses[i]);
                InstructorController instructorController = new InstructorController();
                ActionResult instructorControllerCreateResult = instructorController.Create(objects.Instructors[i], new string[] { objects.Courses[i].CourseID.ToString() });
                StudentController studentController = new StudentController();
                ActionResult studentControllerCreateResult = studentController.Create(objects.Students[i], new string[] { objects.Courses[i].CourseID.ToString() });
            }
        }

        [TestMethod]
        public void AllControllerCreateTest()
        {
            ConfirmDbSetup();
        }

        [TestMethod]
        public void InstructorDelete()
        {
            ConfirmDbSetup();

            for (int i = 0; i < objects.NumberOfDerivativeObjects; i++)
            {
                InstructorController instructorDeleteController = new InstructorController();
                ActionResult instructorControllerDeleteResult = instructorDeleteController.DeleteConfirmed(objects.Instructors[i].ID);
            }

            HowManyInstructors(0);
            HowManyCourseInstructorEntries(0);
            HowManyOfficeAssignments(0);
        }

        [TestMethod]
        public void CourseDelete()
        {
            ConfirmDbSetup();

            for (int i = 0; i < objects.NumberOfDerivativeObjects; i++)
            {
                CourseController courseDeleteController = new CourseController();
                ActionResult courseControllerDeleteResult = courseDeleteController.DeleteConfirmed(objects.Courses[i].CourseID);
            }

            HowManyCourses(0);
            HowManyCourseInstructorEntries(0);
        }

        [TestMethod]
        public void StudentDelete()
        {
            ConfirmDbSetup();

            for (int i = 0; i < objects.NumberOfDerivativeObjects; i++)
            {
                StudentController studentDeleteController = new StudentController();
                ActionResult studentControllerDeleteResult = studentDeleteController.Delete(objects.Students[i].ID);
            }

            HowManyStudents(0);
            DoEnrollmentsExist(false);
        }

        [TestMethod]
        public void DepartmentDelete()
        {
            ConfirmDbSetup();

            DepartmentController departmentDeleteController = new DepartmentController();

            Task<ActionResult> task = departmentDeleteController.DeleteConfirmed(objects.department.DepartmentID);
            task.Wait();

            Assert.AreEqual(TaskStatus.RanToCompletion, task.Status, "department did not delete, task did not complete correctly");

            DoesDepartmentExist(false);
            HowManyCourses(0);
            HowManyInstructors(0);
            HowManyOfficeAssignments(0);
            HowManyCourseInstructorEntries(0);
            DoEnrollmentsExist(false);
            HowManyStudents(objects.NumberOfDerivativeObjects);
        }

        private void DatabaseReset()
        {
            db.Database.Delete();
        }

        private int InstructorCountByCourse(int courseID)
        {
            string query = "select count(InstructorID) from CourseInstructor where CourseID = @CourseID";
            return db.Database.SqlQuery<int>(query, new SqlParameter("@CourseID", courseID)).SingleOrDefault();
        }

        private void ConfirmDbSetup()
        {
            HowManyCourses(objects.NumberOfDerivativeObjects);
            HowManyInstructors(objects.NumberOfDerivativeObjects);
            HowManyOfficeAssignments(objects.NumberOfDerivativeObjects);
            HowManyCourseInstructorEntries(1);
            HowManyStudents(objects.NumberOfDerivativeObjects);
            DoEnrollmentsExist(true);
        }

        private void HowManyStudents(int num)
        {
            Assert.AreEqual<int>(num, db.Students.Where(s => s.EnrollmentDate == objects.studentEnrollmentDate).Count(),
                "incorrect number of students");
        }

        private void DoEnrollmentsExist(bool yesOrNo)
        {
            for (int i = 0; i < objects.NumberOfDerivativeObjects; i++)
            {
                int studentID = objects.Students[i].ID;
                if (yesOrNo)
                    Assert.IsNotNull(db.Enrollments.Where(s => s.StudentID == studentID).SingleOrDefault(),
                        "incorrect number of enrollments");
                else
                    Assert.IsNull(db.Enrollments.Where(s => s.StudentID == studentID).SingleOrDefault(),
                        "incorrect number of enrollments");
            }
        }

        private void DoesDepartmentExist(bool yesOrNo)
        {
            if (yesOrNo)
            Assert.IsNotNull(db.Departments.Where(d => d.Name == objects.department.Name).SingleOrDefault(),
                "department exists");
            else
            Assert.IsNull(db.Departments.Where(d => d.Name == objects.department.Name).SingleOrDefault(),
                "department does not exist");
        }

        private void HowManyOfficeAssignments(int num)
        {
            Assert.AreEqual<int>(num, db.OfficeAssignments.Where(o => o.Location == objects.officeLocation).Count(),
                "not enough office locations deleted");
        }

        private void HowManyCourseInstructorEntries(int num)
        {
            for (int i = 0; i < objects.NumberOfDerivativeObjects; i++)
            {
                Assert.AreEqual<int>(num, InstructorCountByCourse(objects.Courses[i].CourseID),
                    "incorrect number of instructors for course");
            }
        }

        private void HowManyInstructors(int num)
        {
            Assert.AreEqual<int>(num, db.Instructors.Where(i => i.DepartmentID == objects.department.DepartmentID).Count(),
                "incorrect number of instructors");
        }

        private void HowManyCourses(int num)
        {
            Assert.AreEqual<int>(num, db.Courses.Where(i => i.DepartmentID == objects.department.DepartmentID).Count(),
                "incorrect number of courses");
        }

        

        //[TestCleanup()]
        //public void TestCleanup()
        //{

        //}
    }
}