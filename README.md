#    Welcome to the Parliament's brand new HR Management System

## More powered than ever!

Following from the work in my last take home test (which can be found [here](https://github.com/AlSkylark/UK-Parliament-Senior-Home-Exercise)) I've improved significantly the look, performance and usability of the entire application. With special care taken to, this time, fulfill ALL criteria to a higher degree.

To summarise here's a list of changes from the previous iteration:  
* Refactors the Angular `EmployeeService` to not be tightly coupled with the editor's visibility. 
* Adds fully functional create feature. *(Somehow this was missing!?)*
* Adds managerial details to both Employees AND Managers, with the ability to remove an assigned manager from an Employee and to list a Manager's managed employees.
* Adds a Dark mode and a `ThemingService` to toggle between the two. 
* Adds a small alerting service that shows when an employee is saved, created or deleted, or if the operation errored.
* Increases code coverage with a set of integration tests for the Employee CRUD features. 
* Increases code coverage for Angular components, fixes all Jasmine/Karma tests to pass and adds a couple of example tests as well as exhaustive tests on the `Utility` classes.
* Small tweaks on the UI to improve styling and design, including a mock "Sign in" welcome screen and other such details.
* Takes OUT FluentAssertions owed to their change in licensing. 

### Notes on running tests
To run the new Jasmine/Karma tests you need to `cd` into `UkParliament.CodeTest.Web/ClientApp` and manually run the `npm run test` command.  
This will open a new browser where the tests should appear and automatically run.