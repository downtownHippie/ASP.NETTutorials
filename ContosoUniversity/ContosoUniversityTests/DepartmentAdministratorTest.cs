using ContosoUniversity.Controllers;
using ContosoUniversity.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.Entity;

namespace ContosoUniversityTests
{
    [TestClass]
    public class DepartmentAdministratorTest : DatabaseSetup
    {
        [TestMethod]
        public void AdministratorTestWithForeignInsrtuctor()
        {
            ConfirmDbSetup();

            DepartmentController departmentController = new DepartmentController();
            Department department = new Department();
            department.Name = Guid.NewGuid().ToString("D");
            department.Budget = 1m;
            Task<ActionResult> createTask = departmentController.Create(department);
            createTask.Wait();

            // if a department is not correctly created might as well stop the test
            Assert.AreEqual(TaskStatus.RanToCompletion, createTask.Status,
                "department did not make, task did not complete correctly");
            DoesDepartmentExist(department.DepartmentID, true);

            int foreignInstructorID = objects.Instructors[0].ID;
            department.InstructorID = foreignInstructorID;

            DepartmentController departmentEditController = new DepartmentController();
            Task<ActionResult> editTask = departmentEditController.Edit(department);
            editTask.Wait();

            Assert.AreEqual(TaskStatus.RanToCompletion, editTask.Status,
                "department did not edit, task did not complete correctly");

            Assert.IsNull(department.InstructorID,
                "foreign instructor should not be allowed to take over");
        }

        [TestMethod]
        public void AdministratorTestWithInvalidInsrtuctor()
        {
            ConfirmDbSetup();

            objects.department.InstructorID = 99;
            
            DepartmentController departmentEditController = new DepartmentController();
            Task<ActionResult> editTask = departmentEditController.Edit(objects.department);
            editTask.Wait();

            Assert.AreEqual(TaskStatus.RanToCompletion, editTask.Status,
                "department did not edit, task did not complete correctly");

            Assert.IsNull(objects.department.InstructorID,
                "invalid instructor id should not be allowed");
        }

        [TestMethod]
        public void AdministratorTestWithGoodInstructor()
        {
            ConfirmDbSetup();

            objects.department.InstructorID = objects.Instructors[0].ID;

            DepartmentController departmentEditController = new DepartmentController();
            Task<ActionResult> editTask = departmentEditController.Edit(objects.department);
            editTask.Wait();

            Assert.AreEqual(TaskStatus.RanToCompletion, editTask.Status,
                "department did not edit, task did not complete correctly");

            Assert.IsNotNull(objects.department.InstructorID,
                "administrator not set correctly");
        }

        [TestMethod]
        public void AdministratorTestClearInstructor()
        {
            ConfirmDbSetup();

            objects.department.InstructorID = objects.Instructors[0].ID;

            DepartmentController setAdminController = new DepartmentController();
            Task<ActionResult> setAdminTask = setAdminController.Edit(objects.department);
            setAdminTask.Wait();

            Assert.AreEqual(TaskStatus.RanToCompletion, setAdminTask.Status,
                "department did not accept administrator, task did not complete correctly");

            Assert.IsNotNull(objects.department.InstructorID,
                "administrator did not set");

            objects.department.InstructorID = null;
            DepartmentController clearAdminController = new DepartmentController();
            Task<ActionResult> clearAdminTask = setAdminController.Edit(objects.department);
            clearAdminTask.Wait();

            Assert.AreEqual(TaskStatus.RanToCompletion, clearAdminTask.Status,
                "department did not edit, task did not complete correctly");

            Assert.IsNull(objects.department.InstructorID,
                "administrator did not clear");
        }

        [TestMethod]
        public void AdministratorUntestable()
        {
            ConfirmDbSetup();

            Department department = db.Departments.Single(d => d.DepartmentID == objects.department.DepartmentID);

            department.InstructorID = 1;
            db.Entry(department).State = EntityState.Modified;
            db.SaveChanges();

            Assert.IsNull(department.InstructorID,
                "can not stop this");

            // there is probably another test here
            //objects.department.InstructorID = 1;
            //db.Entry(objects.department).State = EntityState.Modified;
            //db.SaveChanges();
        }
    }
}