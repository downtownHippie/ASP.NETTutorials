[Link to the original tutorial on ASP.NET](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc)

1. A mechanism to enroll students in courses was created.
    * The create new student page was modified to list all courses offered by the university.
        1. Selecting courses will enroll a student in those courses.
    * The edit student page was modified to list all courses offered by the university.
        1. The courses a student is enrolled in will be selected.
        1. A student can be enrolled in additional courses by selecting them.
        1. A student can be unenrolled from courses by unselecting them.
1. A mechamism to assign grades to students was created.
    * When viewing the details of an instructor select a course and then select individual students to assigned grades.
1. A [How To](https://github.com/downtownHippie/ASP.NETTutorials/blob/master/ContosoUniversity/HowTo.md) was created to detail the steps to utilize this and test enhanced tutorial.
1. Instructors are now assigned to departments.
    * Instructors can only teach courses offered by their department.
    * Only instructors assigned to a department are eligible to be selected as department administrator.
1. The department detail page now lists all courses and instructors assigned to that department.
1. Model inheritance was implemented through TPC methods.
1. Lazy loading was disabled on the database context.
1. A suite of integration and unit tests has been developed.
1. GitHub's [issue repository](https://github.com/downtownHippie/ASP.NETTutorials/issues) is being utilized to track bugs as well as enhancments to the tutorial.
1. Implemented a readonly webservice.
