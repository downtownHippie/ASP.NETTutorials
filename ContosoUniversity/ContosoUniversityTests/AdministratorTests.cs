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
    public class AdministratorTests : DatabaseSetup
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
            department.AdministratorID = foreignInstructorID;

            DepartmentController departmentEditController = new DepartmentController();
            Task<ActionResult> editTask = departmentEditController.Edit(department);
            editTask.Wait();

            Assert.AreEqual(TaskStatus.RanToCompletion, editTask.Status,
                "department did not edit, task did not complete correctly");

            Assert.IsNull(department.AdministratorID,
                "foreign instructor should not be allowed to take over");
        }

        [TestMethod]
        public void AdministratorTestWithInvalidInsrtuctor()
        {
            ConfirmDbSetup();

            objects.department.AdministratorID = 99;
            
            DepartmentController departmentEditController = new DepartmentController();
            Task<ActionResult> editTask = departmentEditController.Edit(objects.department);
            editTask.Wait();

            Assert.AreEqual(TaskStatus.RanToCompletion, editTask.Status,
                "department did not edit, task did not complete correctly");

            Assert.IsNull(objects.department.AdministratorID,
                "invalid instructor id should not be allowed");
        }

        [TestMethod]
        public void AdministratorTestWithGoodInstructor()
        {
            ConfirmDbSetup();

            objects.department.AdministratorID = objects.Instructors[0].ID;

            DepartmentController departmentEditController = new DepartmentController();
            Task<ActionResult> editTask = departmentEditController.Edit(objects.department);
            editTask.Wait();

            Assert.AreEqual(TaskStatus.RanToCompletion, editTask.Status,
                "department did not edit, task did not complete correctly");

            Assert.IsNotNull(objects.department.AdministratorID,
                "administrator not set correctly");
        }

        [TestMethod]
        public void AdministratorTestClearInstructor()
        {
            ConfirmDbSetup();

            objects.department.AdministratorID = objects.Instructors[0].ID;

            DepartmentController setAdminController = new DepartmentController();
            Task<ActionResult> setAdminTask = setAdminController.Edit(objects.department);
            setAdminTask.Wait();

            Assert.AreEqual(TaskStatus.RanToCompletion, setAdminTask.Status,
                "department did not accept administrator, task did not complete correctly");

            Assert.IsNotNull(objects.department.AdministratorID,
                "administrator did not set");

            objects.department.AdministratorID = null;
            DepartmentController clearAdminController = new DepartmentController();
            Task<ActionResult> clearAdminTask = setAdminController.Edit(objects.department);
            clearAdminTask.Wait();

            Assert.AreEqual(TaskStatus.RanToCompletion, clearAdminTask.Status,
                "department did not edit, task did not complete correctly");

            Assert.IsNull(objects.department.AdministratorID,
                "administrator did not clear");
        }

        [TestMethod]
        public void AdministratorUntestable()
        {
            ConfirmDbSetup();

            //objects.department.InstructorID = 1;

            //DepartmentController departmentEditController = new DepartmentController();
            //Task<ActionResult> editTask = departmentEditController.Edit(objects.department);
            //editTask.Wait();

            //Assert.AreEqual(TaskStatus.RanToCompletion, editTask.Status,
            //    "department did not edit, task did not complete correctly");
            //Assert.IsNull(objects.department.InstructorID,
            //    "invalid instructor id should not be allowed");

            Department department = db.Departments.Single(d => d.DepartmentID == objects.department.DepartmentID);

            Assert.IsNull(department.AdministratorID,
                "there should not be an administrator set");

            department.AdministratorID = 1;
            db.Entry(department).State = EntityState.Modified;
            db.SaveChanges();

            Assert.IsNull(department.AdministratorID,
                "can not stop this");

            //// there is probably another test here
            //objects.department.InstructorID = 1;
            //db.Entry(objects.department).State = EntityState.Modified;
            //db.SaveChanges();
        }
    }
}