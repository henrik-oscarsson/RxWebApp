# RxWebApp
Illustrates how to use Rx in an ASP.NET MVC 6 project.

## Using Rx in an ASP.NET web app ##
If you're like me, you've been using Reactive Extensions (Rx) for years, and wanted to incorporate it into an ASP.NET project also, but didn't know how.
This project demonstrates how. It shows you how to write and set up your familar Rx chain inside an action method.

The project itself is a standard ASP.NET MVC 5 project, the one you get by simply creating a new project in Visual Studio. Nothing has been added, except these parts:
* An orders view
* An orders controller
* An order service and an offer service
* An extension method

### Notes ###
This works just fine in ASP.NET MVC 5 or 6. It works in Visual Studio 2013 and 2015.

### Key features ###
* Using async action methods on controllers
* Using an extension method to convert between Task<T> and IObservable<T>
* Handles 'automatic' Unsubscribe

### Relevant source code ###
* The ObservableExtensions class
* The OrdersController class

### How to run and test ###
* Do a 'git pull'.
* Perform a build in Visual Studio (If it doesn't work out of the box, enable "Nuget Package Restore" and rebuild).
* Run the app, and click on the "Orders" link at the top.
