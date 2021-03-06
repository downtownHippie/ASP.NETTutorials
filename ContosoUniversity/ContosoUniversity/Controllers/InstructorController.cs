﻿using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Instructor
        public ActionResult Index()
        {
            var instructors = db.Instructors
                .OrderBy(i => i.Department.Name)
                .ThenBy(i => i.LastName)
                .ThenBy(i => i.FirstMidName)
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Department);
            return View(instructors);
        }

        // GET: Instructor/Details/5
        public ActionResult Details(int? id, int? courseID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Department)
                .Include("Department.Administrator")
                .Include(i => i.Courses)
                .Include("Courses.Enrollments.Grade")
                .Where(i => i.ID == id)
                .Single();

            if (instructor == null)
            {
                return HttpNotFound();
            }

            var viewModel = new InstructorDetailData();
            viewModel.Instructor = instructor;
            viewModel.Courses = instructor.Courses;

            if (courseID != null)
            {
                ViewBag.CourseID = courseID;
                ViewBag.CourseTitle = db.Courses.Where(c => c.CourseID == courseID).Select(t => t.Title).Single();

                var selectedCourse = instructor.Courses.Where(x => x.CourseID == courseID).Single();
                db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    db.Entry(enrollment).Reference(x => x.Student).Load();
                }
                viewModel.Enrollments = selectedCourse.Enrollments;
            }

            return View(viewModel);
        }

        // GET: Instructor/Create
        public ActionResult Create()
        {
            var instructor = new Instructor();
            PopulateAssignedCourseData(instructor);
            PopulateDepartmentsDropDownList();
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstMidName,HireDate,DepartmentID,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = db.Courses.Find(int.Parse(course));
                    instructor.Courses.Add(courseToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateAssignedCourseData(instructor);
            PopulateDepartmentsDropDownList(instructor.DepartmentID);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Include(i => i.Department)
                .Where(i => i.ID == id)
                .Single();
            if (instructor == null)
            {
                return HttpNotFound();
            }
            PopulateAssignedCourseData(instructor);
            PopulateDepartmentsDropDownList(instructor.DepartmentID);
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructorToUpdate = db.Instructors
               .Include(i => i.OfficeAssignment)
               .Include(i => i.Courses)
               .Include(i => i.Department)
               .Where(i => i.ID == id)
               .Single();

            if (TryUpdateModel(instructorToUpdate, "",
               new string[] { "LastName", "FirstMidName", "HireDate", "DepartmentID", "OfficeAssignment" }))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }

                    UpdateInstructorCourses(instructorToUpdate, selectedCourses);

                    db.Entry(instructorToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedCourseData(instructorToUpdate);
            PopulateDepartmentsDropDownList(instructorToUpdate.DepartmentID);
            return View(instructorToUpdate);
        }

        // GET: Instructor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Department)
                .Single(i => i.ID == id);

            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include("Courses.Instructors")
                .Where(i => i.ID == id)
                .Single();

            db.Instructors.Remove(instructor);

            var department = db.Departments
                .Where(d => d.AdministratorID == id)
                .SingleOrDefault();
            if (department != null)
                department.AdministratorID = null;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = db.Courses.Where(d => d.DepartmentID == instructor.DepartmentID);
            var instrucorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                    {
                        CourseID = course.CourseID,
                        Title = course.Title,
                        Assigned = instrucorCourses.Contains(course.CourseID)
                    });
            }
            ViewBag.Courses = viewModel;
        }

        private void UpdateInstructorCourses(Instructor instructorToUpdate, string[] selectedCourses)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.Courses.Select(c => c.CourseID));
            foreach (var course in db.Courses)
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                        instructorToUpdate.Courses.Add(course);
                }
                else
                    if (instructorCourses.Contains(course.CourseID))
                        instructorToUpdate.Courses.Remove(course);
            }
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = (from d in db.Departments
                                   orderby d.Name
                                   select d).ToList();
            departmentsQuery.Insert(0, null);
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
