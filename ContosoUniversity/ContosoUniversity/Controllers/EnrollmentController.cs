using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace ContosoUniversity.Controllers
{
    public class EnrollmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Enrollment/Edit/5
        public async Task<ActionResult> Edit(int? id, int instructorID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .Include(e => e.Grade)
                .Where(e => e.EnrollmentID == id)
                .SingleAsync();

            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.theCourseID = enrollment.CourseID;
            ViewBag.theInstructorID = instructorID;
            PopulateGradeDropDown(enrollment.GradeID);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,GradeID")] Enrollment enrollment, int instructorID)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Instructor", new { id = instructorID, courseID = enrollment.CourseID });
            }
;
            return View(enrollment);
        }

        private void PopulateGradeDropDown(int? GradeID)
        {
            var gradeQuery = (db.Grades).ToList();
            gradeQuery.Insert(0, null);
            ViewBag.GradeId = new SelectList(gradeQuery, "GradeID", "Letter", GradeID);
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
