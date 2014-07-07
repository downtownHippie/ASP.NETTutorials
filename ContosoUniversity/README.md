In the tutorial there was no mechanism to enroll students in courses (initial enrollments were done via [the seed method](https://github.com/downtownHippie/ASP.NETTutorials/blob/master/ContosoUniversity/ContosoUniversity/Migrations/Configuration.cs) in Configuration.cs).

1. The create new student page was modified to list all courses offered by the university.
    * Checking a course will enroll a student in that course.
2. The edit student page was modified to list all courses offered by the university.
    * The courses a students is enrolled in are checked.
    * Students can be enrolled in additional courses by checking them.
    * Students can be unenrolled from courses by unchecking them.
    
Through the tutorial, the instructors index page allowed selection of an instructor, the page would then repost and list all courses that instructor was teaching.  Then with the selection of one of those courses the page would repost again listing all the students in those courses with their grades.
* All that was moved to the instructor detail page, with similar functionality.

TODO: all todo's were migrated to issues associated with this repository.

[Link to the original tutorial on ASP.NET](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc)

Notes from the original tutorial:

1. LocalDB was not used, the connection string in [Web.Config](https://github.com/downtownHippie/ASP.NETTutorials/blob/master/ContosoUniversity/ContosoUniversity/Web.config) points to a SQLServer 2014 instance named ContosoUniversity.
2. The code in [SchoolInitializer.cs](https://github.com/downtownHippie/ASP.NETTutorials/blob/master/ContosoUniversity/ContosoUniversity/DAL/SchoolInitializer.cs) was commented out later on as a precaution.
3. The code in the seed method and an associated method in [Migrations/Configruations.cs](https://github.com/downtownHippie/ASP.NETTutorials/blob/master/ContosoUniversity/ContosoUniversity/Migrations/Configuration.cs) was commented out.
    * The StartDate field was removed from the department model, the seed method was updated.
    * These methods should adequately seed a database to utilize this project - don't forget to uncomment it if you want to seed a database.
4. [The connection resiliency and command interception](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/connection-resiliency-and-command-interception-with-the-entity-framework-in-an-asp-net-mvc-application) steps were skipped.
5. [The concurrency step](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/handling-concurrency-with-the-entity-framework-in-an-asp-net-mvc-application) was skipped.
6. [The inheritance step](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/implementing-inheritance-with-the-entity-framework-in-an-asp-net-mvc-application) was skipped.
7. [None of the advanced Entity Framework scenarios](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/advanced-entity-framework-scenarios-for-an-mvc-web-application) were implemented.
