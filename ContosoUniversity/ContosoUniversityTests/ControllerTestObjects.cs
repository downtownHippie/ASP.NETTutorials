using ContosoUniversity.Models;
using System;
using System.Collections.Generic;

namespace ContosoUniversityTests
{
    public class ControllerTestObjects
    {
        public int NumberOfDerivativeObjects { get; private set; }
        // just to set some upper limit
        private int MaxDerivedObjects = 10;
        public Department department = new Department();
        public List<Course> Courses = new List<Course>();
        public List<Instructor> Instructors = new List<Instructor>();
        public List<Student> Students = new List<Student>();

        public readonly DateTime instructorHireDate = new DateTime(9999, 12, 31);
        public readonly DateTime studentEnrollmentDate = new DateTime(9999, 12, 31);

        public readonly string officeLocation = Guid.NewGuid().ToString("D");

        public ControllerTestObjects()
        {
            throw new ArgumentOutOfRangeException("numberOfDerivativeObjects", "The Factory needs a number of derived objects to make");
        }

        public ControllerTestObjects(int numberOfDerivativeObjects)
        {
            if ((numberOfDerivativeObjects > MaxDerivedObjects) || (numberOfDerivativeObjects <= 0))
            {
                throw new ArgumentOutOfRangeException("numberOfDerivativeObjects", "The Factory makes from 1 to " + MaxDerivedObjects + " objects");
            }
            NumberOfDerivativeObjects = numberOfDerivativeObjects;
            department.Name = Guid.NewGuid().ToString("D");
            department.Budget = 1m;
            for (int i = 1; i <= numberOfDerivativeObjects; i++)
            {
                Course course = new Course();
                course.CourseID = i;
                course.Credits = 3;
                course.Title = Guid.NewGuid().ToString("D");
                Courses.Add(course);
                Instructor instructor = new Instructor();
                instructor.FirstMidName = Guid.NewGuid().ToString("D");
                instructor.LastName = Guid.NewGuid().ToString("D");
                instructor.HireDate = instructorHireDate;
                instructor.OfficeAssignment = new OfficeAssignment();
                instructor.OfficeAssignment.Location = officeLocation;
                Instructors.Add(instructor);
                Student student = new Student();
                student.FirstMidName = Guid.NewGuid().ToString("D");
                student.LastName = Guid.NewGuid().ToString("D");
                student.EnrollmentDate = studentEnrollmentDate;
                Students.Add(student);
            }
        }

        public void SetDepartmentID(int id)
        {
            for (int i = 0; i < NumberOfDerivativeObjects; i++)
            {
                Courses[i].DepartmentID = id;
                Instructors[i].DepartmentID = id;
            }
        }
    }
}
