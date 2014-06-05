Menu-Master
===========

A tool that integrates with popular, free technology to make responsible food decisions for you, and help you carry them through. This document provides a rough guideline for the requirements.

Design
-------

Three main features will be tied together by a Windows Service.

Google Calendar will provide the functionality for tracking meal plans. Users are able to see what is upcoming and make their own adjustments easily, from anywhere they can access their Google account.

Evernote (may change) provides an effective service for managing lists. Users can access Evernote easily from a variety of places, notably their smartphones, and the program will make sure nothing is forgotten from the list.

The UI will take the form of a webpage. The UI will be accessible from a Notification Icon in the taskbar, as well as the ability to manipulate the underlying service directly. The UI is the most important component, and needs the following features.
* Recipe management: Add, edit and delete. Flag a recipe "appropriate" for different meals (ie. Breakfast or Snack). Meal management is accessible here (add, edit and delete meal names).
* Day management: Add, edit and delete different types of Days (ie Workday or Workday-with-Gym). The Day types are defined by a Name, and an ordered list of the meals they include. Set which Day type is default. Meal management is accessible here (add, edit and delete meal names).
* Meal planning: The meat of the project. Specify a number of "look-ahead" days. Fill each day, up to the look-ahead limit, with different meal plans. The usage pattern goes as follows.
	* While there are days up to the look-ahead limit that don't have any meal plan, present a day planner for each day.
	* The day planner assumes a default Day Type. All other Day Types are accessible via a dropdown box.
	* Whenever a Day Type is set, including the initial default setting, the day planner is populated with a list of meals for that type, as specified in the Day Management page.
	* Each meal is initially set to whatever is specified for that day type, but can be changed via dropdown, or removed with a button. New meals can be added. The meals can be dragged and dropped to change their order.
	* Each meal has a recipe picker, which is simply a dropdown box full of recipes that are flagged as appropriate for that meal type. The default recipes will always be whichever recipes were least recently used, including recipes specified as happening in the same planning step.
	* Each day may be marked as "skipped," which plans no meals and sets no type for that day.
* Configuration settings page:
	* Account info for Google Calendar, and which Calendar is used for meal planning.
	* Account info for Evernote.
	* Ability to clear current data.

The underlying service is responsible for tying the various components together. It will present the UI, populated with data directly from Google Calendar. When the UI is submitted, it will update Google Calendar. When Google Calendar is updated, by the UI or by something else, a shopping list is generated.

TODO: The best way to manage a shopping list needs some fleshing out. Not sure if Evernote is the right choice yet.

Technology List
-------

Microsoft Visual Studio Express; Any version that can create C# classes and edit MVC pages and provides a simple database framework. Express 2013 For Web might be good enough: http://www.visualstudio.com/en-us/products/visual-studio-express-vs.aspx
Google Calendar SDK: https://developers.google.com/google-apps/calendar/
Evernote API: http://dev.evernote.com/doc/

Incremental Steps
-------

**Milestone 1: Google API and Experiments**
* Create a visual studio solution. Check it in to this GitHub repository.
	* Create a simple command line executable that says "Hello World". It's tradition.
	* Anytime ANYTHING works or looks good, check it in to GitHub.
	* Read about Git Branches! Make sure you keep the master branch in a good working place, at all times. Create branches with names like "AddCalendarExecutable" or "CalendarExecutableCreatesEvents", and do your development in there, checking in often. When the purpose of the branch has been fulfilled, and everything is in good working order, check it back into the master branch.
	* Make sure you Git Push up your work periodically. It'll be helpful for me to see progress.
* Make sure you have a dummy Google Account to do Development things with.
* Create a command line executable that will do something, anything with a Google calendar.
	* There should be TWO projects; one contains a "Class Library," with all the code to interact with Google. The other is the command line executable, and it ONLY creates the main object from the class library, and calls functions on that object. It should provide no input to those functions, and receive no output.
	* This makes it really easy to reuse the Class Library code in something else, like a windows service, while keeping the development of the library fast and simple.
* After you can do something petty, build independent functions for Creating a event on a specific calendar and day, reading events on a specific calendar and day, and deleting events on a specific calendar and day.
* At this point, you're probably hard-coding in some Google Account info, and calendar info. Instead, have the executable read in an XML file, and pass things like Username, Password, Calendar name, Day, etc in to the library.

**Milestone 2: The User Interface**
* Add a project to your solution called "CalendarUI". It's type should be "ASP.NET MVC 4 Web Application". When it asks, make it a Blank application. The new project will be full of files and folders that support website development.
* There are three main components to these web applications: Models, Views, and Controllers.
	* Models control and define data. They might connect to a database, perform authentication, or just host some definitions for what data will look like.
	* Views are files that are compiled to HTML. The actual look of a web page is defined by the View.
	* Controllers are the middle man. A URL is parsed by the webserver, and it uses the URL to identify a function in a controller that it should call. The Controller then returns View and Model information that are combined to create a web page.
	* For example, the default way a webserver parses URLs would cause "mywebsite.com/Default/Index" to find the "DefaultController.cs", and call the "Index" function on it. The Index function calls code in the Model section to see if a user is logged in. When they're not, it returns a View that renders to a login page.
* Add a web page to the blank project. Start by making a Controller (in the Controllers folder) called HomeController.cs. It will by default have an Index function, which returns "View()". "View()" can be called with a name of a View (ie View("Index")), but calling it without specifying a name just uses the name of the function, which is already "Index".
* Next you need a View called Index...and it needs to be in the Views\Home folder. Create folders in the Views folder that match the name of the controller that references them; that's how it expects to find those files. The default options for adding a View are fine...your web application doesn't have a "layout" page yet, but you can add one later.
	* The View will have a chunk that sets the ViewBag.Title for the page. If you had a layout page, the layout would reference that variable to determine what to set the title. Currently that setting has no impact though.
* Create some more projects, using non-Blank settings. Look through the sample code that come with these projects and get a feel for how these things look and what they can do.


**Milestone 3: Basic Windows Service**
* Read up on how to create and run a service. You can see it being installed, and whether it's started or stopped in the window's services tool
	* When Googling for any sort of Microsoft-related development technology, I highly recommend looking for results from MSDN.
	* Type "services" into the start menu to bring up the tool.
* Read about Notification Icons. Have the Service present a Notification Icon that can shut it down.

**Milestone 4: The Shopping List API**

**Milestone 5: Adding desired functionality to experimental components**

**Milestone 6: Tying it together in the windows service**