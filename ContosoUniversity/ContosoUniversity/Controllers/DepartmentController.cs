using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContosoUniversity.Controllers
{
    public class DepartmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Department
        public async Task<ActionResult> Index()
        {
            var departments = db.Departments
                .Include(d => d.Administrator)
                .OrderBy(d => d.Name);
            return View(await departments.ToListAsync());
        }

        // GET: Department/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments
                .Include(d => d.Instructors)
                .Include(d => d.Courses)
                .Where(d => d.DepartmentID == id)
                .SingleAsync();
            if (department == null)
            {
                return HttpNotFound();
            }
            var viewModel = new DepartmentDetailData();
            viewModel.Department = department;
            viewModel.Coruses = department.Courses;
            viewModel.Instructors = department.Instructors;
            return View(viewModel);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName");
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartmentID,Name,Budget")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            //ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            PopulateInstructorsDropDownList(department.DepartmentID, department.AdministratorID);
            return View(department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DepartmentID,Name,Budget,AdministratorID")] Department department)
        {
            if (department.AdministratorID != null)
            {
                Instructor potentialAdministrator = db.Instructors.SingleOrDefault(i => i.ID == department.AdministratorID);
                if ((potentialAdministrator == null) || (potentialAdministrator.DepartmentID != department.DepartmentID))
                {
                    department.AdministratorID = null;
                    ModelState.AddModelError("AdministratorID", "Instructor must be valid and assigned to the department");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            PopulateInstructorsDropDownList(department.DepartmentID, department.AdministratorID);
            return View(department);
        }

        // GET: Department/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Department department = await db.Departments
                .Include("Instructors.OfficeAssignment")
                .Include("Courses.Instructors")
                .Where(o => o.DepartmentID == id)
                .SingleAsync();

            db.Departments.Remove(department);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private void PopulateInstructorsDropDownList(int departmentID, object selectedInstructor = null)
        {
            var instructorsQuery = (from i in db.Instructors
                                    where (i.DepartmentID == departmentID)
                                    select i).ToList();
            instructorsQuery.Insert(0, null);
            ViewBag.AdministratorID = new SelectList(instructorsQuery, "ID", "FullName", selectedInstructor);
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
