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

            Assert.IsNotNull(objects.department.InstructorID,
                "administrator not set correctly");
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