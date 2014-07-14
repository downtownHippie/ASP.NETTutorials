﻿using System;
using ContosoUniversity.DAL;
using ContosoUniversity.Controllers;
using ContosoUniversity.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ControllerTests
{
    [TestClass]
    public class ControllerTests
    {
        private SchoolContext db = new SchoolContext();

        private const string departmentName = "testDepartment";
        private const string instructor1FMName = "instructor1FMName";
        private const string instructor1LastName = "instructor2LastName";
        private static readonly DateTime instructor1HireDate = new DateTime(1999, 1, 1);
        private const string instructor2FMName = "instructor2FMName";
        private const string instructor2LastName = "instructor2LastName";
        private static readonly DateTime instructor2HireDate = new DateTime(1998, 1, 1);
        private const string instructorOffice = "testOfficeLocation";
        private const string course1Title = "course1Title";
        private const string course2Title = "course2Title";
        private const int course1ID = 1111;
        private const int course2ID = 2222;
        private const int courseCredits = 3;
        private const string student1FMName = "StudentOneFMName";
        private const string student1LastName = "StudentOneLastName";
        private const string student2FMName = "StudentTwoFMName";
        private const string student2LastName = "StudentTwoLastName";
        private static readonly DateTime studentEnrollmentDate = new DateTime(1900, 1, 1);

        [TestMethod]
        public void DatabaseReset()
        {
            SchoolContext db = new SchoolContext();
            db.Database.Delete();
        }

        [TestMethod]
        public void DepartmentCreate()
        {
            DepartmentController departmentController = new DepartmentController();

            Department department = new Department();
            department.Name = departmentName;
            department.Budget = 1m;
            Task<ActionResult> task = departmentController.Create(department);
            task.Wait();

            // probably uselsss
            Assert.AreEqual(TaskStatus.RanToCompletion, task.Status, "department didn't make, task didn't complete");

            Department createdDepartment = db.Departments.Where(d => d.Name == departmentName).Single();
            Assert.IsNotNull(createdDepartment, "department didn't make");
        }

        [TestMethod]
        public void InstructorCreate()
        {
            Department createdDepartment = db.Departments.Where(d => d.Name == departmentName).Single();

            InstructorController instructorController1 = new InstructorController();
            Instructor instructor1 = new Instructor();
            instructor1.FirstMidName = instructor1FMName;
            instructor1.LastName = instructor1LastName;
            instructor1.HireDate = instructor1HireDate;
            instructor1.DepartmentID = createdDepartment.DepartmentID;
            instructor1.OfficeAssignment = new OfficeAssignment();
            instructor1.OfficeAssignment.Location = instructorOffice;
            ActionResult result1 = instructorController1.Create(instructor1, new string[] { course1ID.ToString() });

            InstructorController instructorController2 = new InstructorController();
            Instructor instructor2 = new Instructor();
            instructor2.FirstMidName = instructor2FMName;
            instructor2.LastName = instructor2LastName;
            instructor2.HireDate = instructor2HireDate;
            instructor2.DepartmentID = createdDepartment.DepartmentID;
            instructor2.OfficeAssignment = new OfficeAssignment();
            instructor2.OfficeAssignment.Location = instructorOffice;
            ActionResult result2 = instructorController2.Create(instructor2, new string[] { course2ID.ToString() });

            var createdInstructors = db.Instructors.Where(i => i.DepartmentID == createdDepartment.DepartmentID);
            Assert.AreEqual<int>(2, createdInstructors.Count(), "not enough instructors created");

            var createdOffices = db.OfficeAssignments.Where(o => o.Location == instructorOffice);
            Assert.AreEqual<int>(2, createdOffices.Count(), "not enough offices created");

            // somehow need to check CourseInstructor table for 2 entries...
            //     make a bool method to check for each course and assert true
            //     will use same method when deleting and then will assert false
            Assert.AreEqual<int>(1, InstructorCountByCourse(course1ID), "incorrect number of instructors for course");
            Assert.AreEqual<int>(1, InstructorCountByCourse(course2ID), "incorrect number of instructors for course");
        }

        private int InstructorCountByCourse(int courseID)
        {
            string query = "select count(InstructorID) from CourseInstructor where CourseID = @CourseID";
            return db.Database.SqlQuery<int>(query, new SqlParameter("@CourseID", courseID)).SingleOrDefault();
        }

        [TestMethod]
        public void CourseCreate()
        {
            Department createdDepartment = db.Departments.Where(d => d.Name == departmentName).Single();

            CourseController courseController1 = new CourseController();
            Course course1 = new Course();
            course1.CourseID = course1ID;
            course1.Title = course1Title;
            course1.Credits = courseCredits;
            course1.DepartmentID = createdDepartment.DepartmentID;
            ActionResult result1 = courseController1.Create(course1);

            CourseController courseController2 = new CourseController();
            Course course2 = new Course();
            course2.CourseID = course2ID;
            course2.Title = course2Title;
            course2.Credits = courseCredits;
            course2.DepartmentID = createdDepartment.DepartmentID;
            ActionResult result2 = courseController2.Create(course2);

            var createdCourses = db.Courses.Where(c => c.DepartmentID == createdDepartment.DepartmentID);
            Assert.AreEqual<int>(2, createdCourses.Count(), "not enough courses created");
        }

        [TestMethod]
        public void StudentCreate()
        {
            StudentController studentController1 = new StudentController();
            Student student1 = new Student();
            student1.FirstMidName = student1FMName;
            student1.LastName = student1LastName;
            student1.EnrollmentDate = studentEnrollmentDate;
            ActionResult result1 = studentController1.Create(student1, new string[] { course1ID.ToString() });

            StudentController studentController2 = new StudentController();
            Student student2 = new Student();
            student2.FirstMidName = student2FMName;
            student2.LastName = student2LastName;
            student2.EnrollmentDate = studentEnrollmentDate;
            ActionResult result2 = studentController2.Create(student2, new string[] { course2ID.ToString() });

            var createdStudents = db.Students.Where(s => s.EnrollmentDate == studentEnrollmentDate);
            Assert.AreEqual<int>(2, createdStudents.Count(), "not enough students created");
        }

        [TestMethod]
        public void DepartmentDelete()
        {
            Department createdDepartment = db.Departments.Where(d => d.Name == departmentName).Single();
            int studentCountBeforeDelete = (db.Students.Where(s => s.EnrollmentDate == studentEnrollmentDate)).Count();

            DepartmentController departmentController = new DepartmentController();

            Task<ActionResult> task = departmentController.DeleteConfirmed(createdDepartment.DepartmentID);
            task.Wait();

            // probably uselsss
            Assert.AreEqual(TaskStatus.RanToCompletion, task.Status, "department didn't delete, task didn't complete");

            Department deletedDepartment = db.Departments.Where(d => d.Name == departmentName).Single();
            Assert.IsNull(deletedDepartment, "department didn't delete");

            var deletedCourses = db.Courses.Where(c => c.DepartmentID == createdDepartment.DepartmentID);
            Assert.AreEqual<int>(0, deletedCourses.Count(), "didn't delete all the courses");

            var deletedInstructors = db.Instructors.Where(i => i.DepartmentID == createdDepartment.DepartmentID);
            Assert.AreEqual<int>(0, deletedInstructors.Count(), "didn't delete all the instructors");

            var checkforDeletedStudents = db.Students.Where(s => s.EnrollmentDate == studentEnrollmentDate);
            Assert.AreEqual<int>(studentCountBeforeDelete, checkforDeletedStudents.Count(), "deleted some students, shouldn't");
        }
    }
}
