using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ContosoUniversity.Controllers
{
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Student
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var students = from s in db.Students
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                    || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(students.ToPagedList(pageNumber, pageSize));
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            var student = new Student();
            student.Enrollments = new List<Enrollment>();
            PopulateAssignedCourseData(student);
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstMidName,EnrollmentDate")] Student student, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                student.Enrollments = new List<Enrollment>();
                foreach (var courseID in selectedCourses)
                {
                    Enrollment enrollment = new Enrollment();
                    int courseIDi = int.Parse(courseID);
                    Course course = db.Courses.Where(c => c.CourseID == courseIDi).Single();
                    enrollment.Course = course;
                    enrollment.Student = student;
                    student.Enrollments.Add(enrollment);
                }
            }
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                // Log error (uncomment dex variable and add a line to write to log
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator");
            }

            PopulateAssignedCourseData(student);
            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Student student = db.Students.Find(id);
            Student student = db.Students
                .Include(i => i.Enrollments)
                .Where(i => i.ID == id)
                .Single();
            PopulateAssignedCourseData(student);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
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

            var studentToUpdate = db.Students
                .Include(i => i.Enrollments)
                .Where(i => i.ID == id)
                .Single();
            if (TryUpdateModel(studentToUpdate, "", new string[] { "LastName", "FirstMidName", "EnrollmentDate" }))
            {
                try
                {
                    UpdateStudentCourses(selectedCourses, studentToUpdate);
                    db.Entry(studentToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException dex)
                {
                    // log the error (uncomment dex and add line here to log)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists contact your system administrator");
                    ModelState.AddModelError("", dex.Message);
                    if (dex.InnerException != null)
                    {
                        ModelState.AddModelError("", dex.InnerException.Message);
                    }
                }
            }
            PopulateAssignedCourseData(studentToUpdate);
            return View(studentToUpdate);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists contact your system administrator";
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Student student = db.Students.Find(id);

            try
            {
                db.Students.Remove(student);
                db.SaveChanges();
            }
            catch (DataException /* dex */)
            {
                // log error (uncomment dex and add a line here to log error)
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        private void PopulateAssignedCourseData(Student student)
        {
            var allCourses = db.Courses.OrderBy(c => c.CourseID);
            var studentCourses = new HashSet<int>(student.Enrollments.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    DepartmentID = course.DepartmentID,
                    Assigned = studentCourses.Contains(course.CourseID)
                });
            }
            ViewBag.Courses = viewModel;
            ViewBag.Departments = db.Departments.OrderBy(d => d.Name);
        }

        private void UpdateStudentCourses(string[] selectedCourses, Student studentToUpdate)
        {
            HashSet<string> selectedCoursesHS;
            if (selectedCourses == null)
            {
                selectedCoursesHS = new HashSet<string>();
            }
            else
            {
                selectedCoursesHS = new HashSet<string>(selectedCourses);
            }
            var studentEnrollments = new HashSet<int>(studentToUpdate.Enrollments.Select(e => e.CourseID));
            var courses = db.Courses.ToList();
            foreach (var course in courses)
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    if (!studentEnrollments.Contains(course.CourseID))
                    {
                        Enrollment enrollment = new Enrollment();
                        enrollment.Course = course;
                        enrollment.Student = studentToUpdate;
                        studentToUpdate.Enrollments.Add(enrollment);
                    }
                }
                else
                    if (studentEnrollments.Contains(course.CourseID))
                    {
                        Enrollment enrollment = db.Enrollments
                            .Where(i => i.StudentID == studentToUpdate.ID)
                            .Where(d => d.CourseID == course.CourseID)
                            .Single();
                        db.Enrollments.Remove(enrollment);
                    }
            }
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
