using ContosoUniversity.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace ContosoUniversity.DAL
{
    public class SchoolContext : DbContext
    {
        public SchoolContext()
            : base("SchoolContext")
        {
            // this is a hack to support localDb
            if (Database.Connection.ConnectionString.Contains("LocalDb"))
            {
                string[] connectionStringComponents = Database.Connection.ConnectionString.Split(';');
                string dbFileName = connectionStringComponents.Where(i => i.Contains("AttachDBFilename")).Single().Split('=')[1];

                if (!System.IO.File.Exists(dbFileName))
                {
                    string emptyDbfileName = new System.Text.RegularExpressions.Regex("University").Replace(dbFileName, "UniversityEmpty");
                    System.IO.File.Copy(emptyDbfileName, dbFileName);
                }
            }
            Database.SetInitializer<SchoolContext>(new SchoolInitializer());
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseID")
                .MapRightKey("InstructorID")
                .ToTable("CourseInstructor"));

            modelBuilder.Entity<Department>().MapToStoredProcedures();

            modelBuilder.Entity<Department>()
                .HasOptional(o => o.Administrator);
 
            modelBuilder.Entity<Department>()
                .HasMany(o => o.Instructors)
                .WithRequired(i => i.Department)
                .HasForeignKey(i => i.DepartmentID)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Student>()
                .Map(m => m.MapInheritedProperties());

            modelBuilder.Entity<Instructor>()
                .Map(m => m.MapInheritedProperties());
        }
    }
}