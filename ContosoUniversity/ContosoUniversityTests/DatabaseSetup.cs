using ContosoUniversity.Controllers;
using ContosoUniversity.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Data.SqlClient;

namespace ContosoUniversityTests
{
    public class DatabaseSetup
    {
        protected SchoolContext db;
        protected ControllerTestObjects objects;

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
            
            DoesDepartmentExist(objects.department.DepartmentID, true);

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

        protected void DatabaseReset()
        {
            db.Database.Delete();
        }

        protected int InstructorCountByCourse(int courseID)
        {
            string query = "select count(InstructorID) from CourseInstructor where CourseID = @CourseID";
            return db.Database.SqlQuery<int>(query, new SqlParameter("@CourseID", courseID)).SingleOrDefault();
        }

        protected void HowManyStudents(int num)
        {
            Assert.AreEqual<int>(num, db.Students.Where(s => s.EnrollmentDate == objects.studentEnrollmentDate).Count(),
                "incorrect number of students");
        }

        protected void HowManyOfficeAssignments(int num)
        {
            Assert.AreEqual<int>(num, db.OfficeAssignments.Where(o => o.Location == objects.officeLocation).Count(),
                "not enough office locations deleted");
        }

        protected void HowManyCourseInstructorEntries(int num)
        {
            for (int i = 0; i < objects.NumberOfDerivativeObjects; i++)
            {
                Assert.AreEqual<int>(num, InstructorCountByCourse(objects.Courses[i].CourseID),
                    "incorrect number of instructors for course");
            }
        }

        protected void HowManyInstructors(int num)
        {
            Assert.AreEqual<int>(num, db.Instructors.Where(i => i.DepartmentID == objects.department.DepartmentID).Count(),
                "incorrect number of instructors");
        }

        protected void HowManyCourses(int num)
        {
            Assert.AreEqual<int>(num, db.Courses.Where(i => i.DepartmentID == objects.department.DepartmentID).Count(),
                "incorrect number of courses");
        }

        protected void DoEnrollmentsExist(bool yesOrNo)
        {
            int[] studentIds = objects.Students.Select(o => o.ID).ToArray();
            if (yesOrNo)
                Assert.IsTrue(db.Enrollments.Where(s => studentIds.Contains(s.StudentID)).Count() != 0/*.SingleOrDefault()*/,
                    "incorrect number of enrollments");
            else
                Assert.IsTrue(db.Enrollments.Where(s => studentIds.Contains(s.StudentID)).Count() == 0/*.SingleOrDefault()*/,
                    "incorrect number of enrollments");
        }

        protected void DoesDepartmentExist(int id, bool yesOrNo)
        {
            if (yesOrNo)
                Assert.IsNotNull(db.Departments.Where(d => d.DepartmentID == id).SingleOrDefault(),
                    "department exists");
            else
                Assert.IsNull(db.Departments.Where(d => d.DepartmentID == id).SingleOrDefault(),
                    "department does not exist");
        }

        protected void ConfirmDbSetup()
        {
            HowManyCourses(objects.NumberOfDerivativeObjects);
            HowManyInstructors(objects.NumberOfDerivativeObjects);
            HowManyOfficeAssignments(objects.NumberOfDerivativeObjects);
            HowManyCourseInstructorEntries(1);
            HowManyStudents(objects.NumberOfDerivativeObjects);
            DoEnrollmentsExist(true);
        }

        //[TestCleanup()]
        //public void TestCleanup()
        //{

        //}
    }
}
