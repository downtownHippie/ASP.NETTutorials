In order to download and utilize the ContosoUniversity tutorial you will either need to setup LocalDb or have an existing SQL Server instance.

1. Download the repository.
    * Navigate to the main page for the [repository](https://github.com/downtownHippie/ASP.NETTutorials), at the bottom of the right hand column is a button labled Download ZIP.
    * Click that button and save the zip file on your local hard drive.
2. Get the Visual Studio solution out of the zip archive.
    * Open the zip file in explorer and navigate to the directory containing the ContosoUniversity solution.
    * Pick the ContosoUniversity folder, right click and select copy.
3. Paste the solution folder somewhere on your hard drive.
4. Start Visual Studio.
    * Open the folder you just created and double click the ContosoUniversity.sln solution file.
5. You may get 2 security warnings about only opening projects from trustworthy sources.
    * Click OK on both dialog boxes.
6. Configure the database connection string in the Web.config file for the ContosoUniversity project.
    * If you have a running SQL Server instance
        1. Edit the Web.config file.
        2. Change the name of the SQL Server from ZANZABAR to the hostname of your SQL Server.
    * If you want to use the LocalDb option.
        1. Comment out the first connection string and uncomment the second.
7. Configure the database connection string in the App.config file for the ControllerTests project.
    * If you have a running SQL Server instance.
        1. Edit the App.config file.
        2. Change the name of the SQL Server from ZANZABAR to the hostname of your SQL Server.
    * If you want to use the LocalDb option.
        1. Comment out the first connection string and uncomment the second one.
        2. Change the text <PATH> to the path where you copied the solution directory.
8. If you are using LocalDb options create the LocalDb files.
    * Select the App_Data folder in the ContosoUniversity project.
        1. Right click, go to Add and, select New Item.
        2. Pick Data from the list on the left and SQL Server Database from the center panel.
        3. Set the name of the database file to ContosoUniversity.mdf.
        4. Click add.
            * This will create the database file and the log file.
    * Select the ControllerTests project entry in the Solution Explorer.
        1. Right click, go to Add, and select New folder.
        2. Name the new folder App_Data.
        3. Select the new folder App_Data, right click, pick Add and select New Item.
        4. Pick Data from the list on the left and Service-based Database from the center panel.
        5. Set the name of the database file to ContosoUniversityText.mdf.
        6. Click add.
            * This will create the database file.
            * The References folder will probably open, you can collapse this.
9. Save all files.
    * On the FILE menu select Save All.
10. Build the solution.
    * On the BUILD menu select Build Solution.
    * This will add the entity framework and paged list packages.
11. Run the test.
    * In the Test Explorer window selec the departmentdelete test.
        1. It is an orderedtest, do not select DepartmentDelete, the ControllerTests method.
        2. This should take less than 10 seconds.
12. Run the application.
    * Press F5.
13. See the [Readme](https://github.com/downtownHippie/ASP.NETTutorials/blob/master/ContosoUniversity/README.md) file for details about the modifications that were made to the tutorial
    * Enjoy.
        1. Naviage around with the links at the top.
        2. Create new departments, instructors, courses, students.
        3. Assign instructors to courses.
        4. Assign administators to departments.
        5. Give students grades.
    * Be sure check out the issues associated with this project for any open issues.
    * Also, check the closed issues to see how some of the enhacements and bugs were resolved.
