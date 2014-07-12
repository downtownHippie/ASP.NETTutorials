[Link to the original tutorial on ASP.NET](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc)

In the tutorial there was no mechanism to enroll students in courses.  The create new student page was modified to list all courses offered by the university.  Selecting a course will enroll a student in that course.  The edit student page was also modified to list all courses offered by the university.  The courses a student is enrolled in will be selected.  A student can be enrolled in additional courses by selecting them.  A student can be unenrolled from courses by unselecting them.
    
Through the tutorial, the instructors index page allowed selection of an instructor.  The page would then repost and list all courses that instructor was teaching below the insturctor list.  Then, with the selection of one of those courses the page would repost again listing all the students in those courses, with their grades, at the bottom of the page.  All that functionality was moved to the instructor detail page.

When selecting a course on the instructor details page the page reposts with the students and their grades listed at the bottom (as indicated above).  A link (through a controller and view) was added to assign student grades.

CodeFirst migrations were disabled.  [DAL/SchoolInitializer.cs](https://github.com/downtownHippie/ASP.NETTutorials/blob/master/ContosoUniversity/ContosoUniversity/DAL/SchoolInitializer.cs) was modified so that if there is no database the entity framework will create one and seed it with data.  This should go a long way to allowing someone to download and play with this codebase.

When a new department is created it will not have an administrator, after creating some instructors for the new department one of them can be selected as department administrator on the department edit page.

Instructors must now assigned to a department when they are created.  Instructors can now only teach classes offered by their department.

Notes from the original tutorial:

1. LocalDB was not used, the connection string in [Web.Config](https://github.com/downtownHippie/ASP.NETTutorials/blob/master/ContosoUniversity/ContosoUniversity/Web.config) points to a SQLServer 2014 instance named ContosoUniversity.
2. The code in the seed method in [SchoolInitializer.cs](https://github.com/downtownHippie/ASP.NETTutorials/blob/master/ContosoUniversity/ContosoUniversity/DAL/SchoolInitializer.cs) was commented out later on as a precaution.  **NO LONGER RELEVANT**
3. The code in the seed method, and an associated method, in [Migrations/Configruations.cs](https://github.com/downtownHippie/ASP.NETTutorials/blob/master/ContosoUniversity/ContosoUniversity/Migrations/Configuration.cs) was commented out.  **NO LONGER RELEVANT**
    * The StartDate field was removed from the department model, the seed method was updated.
    * These methods should adequately seed a database to utilize this project - don't forget to uncomment it if you want to seed a database.
4. [The connection resiliency and command interception](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/connection-resiliency-and-command-interception-with-the-entity-framework-in-an-asp-net-mvc-application) steps were skipped.
5. [The concurrency step](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/handling-concurrency-with-the-entity-framework-in-an-asp-net-mvc-application) was skipped.
6. [The inheritance step](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/implementing-inheritance-with-the-entity-framework-in-an-asp-net-mvc-application) was skipped.
7. [None of the advanced Entity Framework scenarios](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/advanced-entity-framework-scenarios-for-an-mvc-web-application) were implemented.

**TODO**: all todo's were migrated to issues/enhnacements associated with this repository.
