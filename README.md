Menu-Master
===========

A tool that integrates with popular, free technology to make responsible food decisions for you, and help you carry them through. This document provides a rough guideline for the requirements.

Design
-------

Three main features will be tied together by a Windows Service.

Google Calendar will provide the functionality for tracking meal plans and shopping trips. Users are able to see what is upcoming and make their own adjustments easily, from anywhere they can access their Google account.

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

Technical Components
-------



Incremental Steps
-------
