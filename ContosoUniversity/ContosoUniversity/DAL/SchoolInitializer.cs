using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContosoUniversity.DAL
{
    public class SchoolInitializer : System.Data.Entity.CreateDatabaseIfNotExists<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            context.Database.ExecuteSqlCommand(Properties.Resources.GetGPA);
            context.Database.ExecuteSqlCommand(Properties.Resources.UpdateGPA);
            context.Database.ExecuteSqlCommand(Properties.Resources.UpdateGPAFromInsertTrigger);
            context.Database.ExecuteSqlCommand(Properties.Resources.UpdateGPAFromUpdateDeleteTrigger);
            context.Database.ExecuteSqlCommand(Properties.Resources.CreateGPAFromStudentTrigger);

            var grades = new List<Grade>() {
                new Grade {
                    Letter = "A",
                    Value = 4
                },
                new Grade {
                    Letter = "B",
                    Value = 3
                },
                new Grade {
                    Letter = "C",
                    Value = 2
                },
                new Grade {
                    Letter = "D",
                    Value = 1
                },
                new Grade {
                    Letter = "F",
                    Value = 0
                }
            };

            grades.ForEach(g => context.Grades.Add(g));
            context.SaveChanges();

            var departments = new List<Department>
                {
                    new Department { Name = "English",     Budget = 350000, 
                        /* AdministratorID  = instructors.Single( i => i.LastName == "Abercrombie").ID */ },
                    new Department { Name = "Mathematics", Budget = 100000, 
                        /* AdministratorID  = instructors.Single( i => i.LastName == "Fakhouri").ID */ },
                    new Department { Name = "Engineering", Budget = 350000, 
                        /* AdministratorID  = instructors.Single( i => i.LastName == "Harui").ID */ },
                    new Department { Name = "Economics",   Budget = 100000, 
                        /* AdministratorID  = instructors.Single( i => i.LastName == "Kapoor").ID */ }
                };
            departments.ForEach(s => context.Departments.Add(s));
            context.SaveChanges();

            var instructors = new List<Instructor>
                {
                    new Instructor { FirstMidName = "Kim",     LastName = "Abercrombie", 
                        HireDate = DateTime.Parse("1995-03-11"), 
                    DepartmentID = departments.Single(i => i.Name == "English").DepartmentID },
                    new Instructor { FirstMidName = "Fadi",    LastName = "Fakhouri",    
                        HireDate = DateTime.Parse("2002-07-06"),
                    DepartmentID = departments.Single(i => i.Name == "Mathematics").DepartmentID },
                    new Instructor { FirstMidName = "Roger",   LastName = "Harui",       
                        HireDate = DateTime.Parse("1998-07-01"),
                    DepartmentID = departments.Single(i => i.Name == "Mathematics").DepartmentID },
                    new Instructor { FirstMidName = "Candace", LastName = "Kapoor",      
                        HireDate = DateTime.Parse("2001-01-15"),
                    DepartmentID = departments.Single(i => i.Name == "Engineering").DepartmentID },
                    new Instructor { FirstMidName = "Roger",   LastName = "Zheng",      
                        HireDate = DateTime.Parse("2004-02-12"),
                    DepartmentID = departments.Single(i => i.Name == "Economics").DepartmentID }
                };
            instructors.ForEach(s => context.Instructors.Add(s));
            context.SaveChanges();

            var courses = new List<Course>
                {
                    new Course {CourseID = 1050, Title = "Chemistry",      Credits = 3,
                      DepartmentID = departments.Single( s => s.Name == "Engineering").DepartmentID,
                      Instructors = new List<Instructor>() 
                    },
                    new Course {CourseID = 4022, Title = "Microeconomics", Credits = 3,
                      DepartmentID = departments.Single( s => s.Name == "Economics").DepartmentID,
                      Instructors = new List<Instructor>() 
                    },
                    new Course {CourseID = 4041, Title = "Macroeconomics", Credits = 3,
                      DepartmentID = departments.Single( s => s.Name == "Economics").DepartmentID,
                      Instructors = new List<Instructor>() 
                    },
                    new Course {CourseID = 1045, Title = "Calculus",       Credits = 4,
                      DepartmentID = departments.Single( s => s.Name == "Mathematics").DepartmentID,
                      Instructors = new List<Instructor>() 
                    },
                    new Course {CourseID = 3141, Title = "Trigonometry",   Credits = 4,
                      DepartmentID = departments.Single( s => s.Name == "Mathematics").DepartmentID,
                      Instructors = new List<Instructor>() 
                    },
                    new Course {CourseID = 2021, Title = "Composition",    Credits = 3,
                      DepartmentID = departments.Single( s => s.Name == "English").DepartmentID,
                      Instructors = new List<Instructor>() 
                    },
                    new Course {CourseID = 2042, Title = "Literature",     Credits = 4,
                      DepartmentID = departments.Single( s => s.Name == "English").DepartmentID,
                      Instructors = new List<Instructor>() 
                    },
                };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();

            AddInstructor(context, "Chemistry", "Kapoor");
            AddInstructor(context, "Microeconomics", "Zheng");
            AddInstructor(context, "Macroeconomics", "Zheng");
            AddInstructor(context, "Calculus", "Fakhouri");
            AddInstructor(context, "Trigonometry", "Harui");
            AddInstructor(context, "Composition", "Abercrombie");
            AddInstructor(context, "Literature", "Abercrombie");
            context.SaveChanges();

            var officeAssignments = new List<OfficeAssignment>
                {
                    new OfficeAssignment { 
                        InstructorID = instructors.Single( i => i.LastName == "Fakhouri").ID, 
                        Location = "Smith 17" },
                    new OfficeAssignment { 
                        InstructorID = instructors.Single( i => i.LastName == "Harui").ID, 
                        Location = "Gowan 27" },
                    new OfficeAssignment { 
                        InstructorID = instructors.Single( i => i.LastName == "Kapoor").ID, 
                        Location = "Thompson 304" },
                };
            officeAssignments.ForEach(s => context.OfficeAssignments.Add(s));
            context.SaveChanges();

            var students = new List<Student>
                {
                    new Student { FirstMidName = "Carson",   LastName = "Alexander", 
                        EnrollmentDate = DateTime.Parse("2010-09-01") },
                    new Student { FirstMidName = "Meredith", LastName = "Alonso",    
                        EnrollmentDate = DateTime.Parse("2012-09-01") },
                    new Student { FirstMidName = "Arturo",   LastName = "Anand",     
                        EnrollmentDate = DateTime.Parse("2013-09-01") },
                    new Student { FirstMidName = "Gytis",    LastName = "Barzdukas", 
                        EnrollmentDate = DateTime.Parse("2012-09-01") },
                    new Student { FirstMidName = "Yan",      LastName = "Li",        
                        EnrollmentDate = DateTime.Parse("2012-09-01") },
                    new Student { FirstMidName = "Peggy",    LastName = "Justice",   
                        EnrollmentDate = DateTime.Parse("2011-09-01") },
                    new Student { FirstMidName = "Laura",    LastName = "Norman",    
                        EnrollmentDate = DateTime.Parse("2013-09-01") },
                    new Student { FirstMidName = "Nino",     LastName = "Olivetto",  
                        EnrollmentDate = DateTime.Parse("2005-09-01") }
                };
            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();

            var enrollments = new List<Enrollment>
                {
                    new Enrollment { 
                        StudentID = students.Single(s => s.LastName == "Alexander").ID, 
                        CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID, 
                        GradeID = grades.Single(g => g.Letter == "A").GradeID
                    },
                     new Enrollment { 
                        StudentID = students.Single(s => s.LastName == "Alexander").ID,
                        CourseID = courses.Single(c => c.Title == "Microeconomics" ).CourseID, 
                        GradeID = grades.Single(g => g.Letter == "C").GradeID 
                     },                            
                     new Enrollment { 
                        StudentID = students.Single(s => s.LastName == "Alexander").ID,
                        CourseID = courses.Single(c => c.Title == "Macroeconomics" ).CourseID, 
                        GradeID = grades.Single(g => g.Letter == "B").GradeID 
                     },
                     new Enrollment { 
                         StudentID = students.Single(s => s.LastName == "Alonso").ID,
                        CourseID = courses.Single(c => c.Title == "Calculus" ).CourseID, 
                        GradeID = grades.Single(g => g.Letter == "B").GradeID 
                     },
                     new Enrollment { 
                         StudentID = students.Single(s => s.LastName == "Alonso").ID,
                        CourseID = courses.Single(c => c.Title == "Trigonometry" ).CourseID, 
                        GradeID = grades.Single(g => g.Letter == "B").GradeID 
                     },
                     new Enrollment {
                        StudentID = students.Single(s => s.LastName == "Alonso").ID,
                        CourseID = courses.Single(c => c.Title == "Composition" ).CourseID, 
                        GradeID = grades.Single(g => g.Letter == "B").GradeID 
                     },
                     new Enrollment { 
                        StudentID = students.Single(s => s.LastName == "Anand").ID,
                        CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID
                     },
                     new Enrollment { 
                        StudentID = students.Single(s => s.LastName == "Anand").ID,
                        CourseID = courses.Single(c => c.Title == "Microeconomics").CourseID,
                        GradeID = grades.Single(g => g.Letter == "B").GradeID
                     },
                    new Enrollment { 
                        StudentID = students.Single(s => s.LastName == "Barzdukas").ID,
                        CourseID = courses.Single(c => c.Title == "Chemistry").CourseID,
                        GradeID = grades.Single(g => g.Letter == "F").GradeID
                     },
                     new Enrollment { 
                        StudentID = students.Single(s => s.LastName == "Li").ID,
                        CourseID = courses.Single(c => c.Title == "Composition").CourseID,
                        GradeID = grades.Single(g => g.Letter == "C").GradeID
                     },
                     new Enrollment { 
                        StudentID = students.Single(s => s.LastName == "Justice").ID,
                        CourseID = courses.Single(c => c.Title == "Literature").CourseID,
                        GradeID = grades.Single(g => g.Letter == "B").GradeID
                     }
                };
            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments.Where(
                    s =>
                         s.Student.ID == e.StudentID &&
                         s.Course.CourseID == e.CourseID).SingleOrDefault();
                if (enrollmentInDataBase == null)
                {
                    context.Enrollments.Add(e);
                }
            }
            context.SaveChanges();
        }

        void AddInstructor(SchoolContext context, string courseTitle, string instructorName)
        {
            var crs = context.Courses.SingleOrDefault(c => c.Title == courseTitle);
            var inst = crs.Instructors.SingleOrDefault(i => i.LastName == instructorName);
            if (inst == null)
                crs.Instructors.Add(context.Instructors.Single(i => i.LastName == instructorName));
        }
    }
}