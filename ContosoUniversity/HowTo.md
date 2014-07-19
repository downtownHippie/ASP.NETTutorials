In order to download and utilize my extended ContosoUniversity tutorial you will need an existing SQL Server instance.

1. Download the repository.
    * Navigate to the main page for the [repository](https://github.com/downtownHippie/ASP.NETTutorials), at the bottom of the right hand column is a button labled Download ZIP.
    * Click that button and save the zip file on your local hard drive.
1. Get the Visual Studio solution out of the zip archive.
    * Open the zip file in explorer and navigate to the directory containing the ContosoUniversity solution.
    * Pick the ContosoUniversity folder, right click and select copy.
1. Paste the solution folder somewhere on your hard drive.
1. Start Visual Studio.
    * Open the folder you just created and double click the ContosoUniversity.sln solution file.
1. You may get security warnings about only opening projects from trustworthy sources.
    * Click OK in the dialog boxes.
1. Configure the database connection string in the Web.config file for the ContosoUniversity project.
    * Edit the Web.config file.
    * Change the name of the SQL Server from ZANZABAR to the hostname of your SQL Server.
1. Configure the database connection string in the App.config file for the ContosoUniversityTests project.
    * Edit the App.config file.
    * Change the name of the SQL Server from ZANZABAR to the hostname of your SQL Server.
1. Save all files.
    * On the FILE menu select Save All.
1. Build the solution.
    * On the BUILD menu select Build Solution.
    * This will add the entity framework and paged list packages.
1. Run the tests.
    * In the Test Explorer window select the option to run all tests.
1. Run the application.
    * From the DEBUG menu select Start Without Debugging.
1. See the [Readme](https://github.com/downtownHippie/ASP.NETTutorials/blob/master/ContosoUniversity/README.md) file for details about the modifications that were made to the tutorial
    * Enjoy.
        1. Naviage around with the links at the top.
        1. Create new departments, instructors, courses, students.
        1. Assign instructors to courses.
        1. Assign administators to departments.
        1. Give students grades.
    * Be sure check out the [issues](https://github.com/downtownHippie/ASP.NETTutorials/issues) associated with this project for any open issues.
    * Also, check the closed issues to see how some of the enhacements and bugs were resolved.
