using ContosoUniversity.Controllers;
using ContosoUniversity.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

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
            
            // crap!
            //DoesDepartmentExist(true);

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

            // This is a hack to support LocalDb
            // when a LocalDb is deleted the file is removed from the filesystem
            // here we determine if the connection string is one of LocalDb type
            // if so, determine if the AttachDBFilename file exists
            // if not copy the default into place.
            if (db.Database.Connection.ConnectionString.Contains("LocalDb"))
            {
                string[] connectionStringComponents = db.Database.Connection.ConnectionString.Split(';');
                string dbFileName = connectionStringComponents.Where(i => i.Contains("AttachDBFilename")).Single().Split('=')[1];

                if (!System.IO.File.Exists(dbFileName))
                {
                    string emptyDbfileName = new System.Text.RegularExpressions.Regex("Test").Replace(dbFileName, "Empty");
                    System.IO.File.Copy(emptyDbfileName, dbFileName);
                }
            }
        }

        //[TestCleanup()]
        //public void TestCleanup()
        //{

        //}
    }
}
