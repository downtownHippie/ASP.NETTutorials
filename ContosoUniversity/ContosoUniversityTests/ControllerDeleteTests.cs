﻿using ContosoUniversity.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContosoUniversityTests
{
    [TestClass]
    public class ControllerDeleteTests : DatabaseSetup
    {
        [TestMethod]
        public void InstructorDelete()
        {
            ConfirmDbSetup();

            for (int i = 0; i < objects.NumberOfDerivedObjects; i++)
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

            for (int i = 0; i < objects.NumberOfDerivedObjects; i++)
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

            for (int i = 0; i < objects.NumberOfDerivedObjects; i++)
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

            DoesDepartmentExist(objects.department.DepartmentID, false);
            HowManyCourses(0);
            HowManyInstructors(0);
            HowManyOfficeAssignments(0);
            HowManyCourseInstructorEntries(0);
            DoEnrollmentsExist(false);
            HowManyStudents(objects.NumberOfDerivedObjects);
        }
    }
}