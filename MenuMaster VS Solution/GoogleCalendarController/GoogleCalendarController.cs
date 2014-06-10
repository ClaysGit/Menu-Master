using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;

using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

//Reference our hand-crafted OAuthManager to get tokens for us.
using OAuthManager;

namespace GoogleCalendarController
{
    public class googleCalendarController
    {
        string token;
        CalendarService service;

        // This is the initializer for our controller. It creates the main CalendarService, from which most of the magic happens
        // this also fires up our Open Auth Manager, which logs the user into their Google account and generates an access token
        public googleCalendarController()
        {
            //Use the OAuthManager form to get an OAuth token...
            using (var tokenForm = new OAuthTokenManagerForm())
            {
                Application.EnableVisualStyles();
                Application.Run(tokenForm);
                token = tokenForm.OAuthToken;
            }

            //Create the Google Service object for Google Calendars
            service = new CalendarService(new BaseClientService.Initializer()
            {
                ApplicationName = "MenuMaster"
            });
        }
        // Generates a new Calendar object
        public Calendar newCalendar(string summary, string timeZone)
        {
            // Create a new calendar object
            Calendar newCal = new Calendar();

            // Add some parameters. Needs expanding.
            newCal.Summary = summary;
            newCal.TimeZone = timeZone;

            // Create a request object, set the OAuth token, and execute
            var newCalRequest = service.Calendars.Insert(newCal);
            newCalRequest.OauthToken = token;
            return newCalRequest.Execute();

        }
        // Adds an existing calendar to the list of available calendars
        public CalendarListEntry addCalendar(Calendar addCal)
        {
            // Create a new entry object, then set the Id field to that of the given Calendar. This tells the API which Calendar object
            // we are trying to add
            CalendarListEntry calEntry = new CalendarListEntry();
            calEntry.Id = addCal.Id;

            // Now we create an insert request object. This holds all the data we need to actually execute our desired action
            var calInsertRequest = service.CalendarList.Insert(calEntry);
            // Set the authorization token
            calInsertRequest.OauthToken = token;
            // And now we execute!
            return calInsertRequest.Execute();
        }
        // Generates a new Event object
        public Event newEvent(string summary, string location, string calendarID)
        {
            // Create a new event and set some parameters
            Event newEvent = new Event();

            EventDateTime start = new EventDateTime();
            EventDateTime end = new EventDateTime();

            start.DateTimeRaw = "2015-01-01T00:00:00-05:00";
            end.DateTimeRaw = "2015-01-01T12:00:00-05:00";

            newEvent.Start = start;
            newEvent.End = end;

            newEvent.Summary = summary;
            newEvent.Location = location;

            // Create our request object, then set our authorization token before executing
            var eventRequest = service.Events.Insert(newEvent, calendarID);
            eventRequest.OauthToken = token;
            return eventRequest.Execute();
        }
        // Returns a list of calendars associated with the current user
        public CalendarList listCalendars()
        {
            var calendarListRequest = service.CalendarList.List();
            calendarListRequest.OauthToken = token;
            return calendarListRequest.Execute();
        }
        // Find a calendar in a calendar list
        public Calendar findCalendar(CalendarList calList, string findName)
        {
            // Loop through the CalendarListEntry items in our list looking for our specific title
            string ID = "";
            foreach (var item in calList.Items)
            {
                if (item.Summary == findName)
                {
                    // item.Id is the identifier for our calendar. As CalendarListEntry just holds
                    // Calendar MetaData, we need to do in order to request the actual Calendar data below
                    ID = item.Id;
                    break;
                }
            }

            // Craft our request for a specific Calendar object, authorize, and execute
            var getRequest = service.Calendars.Get(ID);
            getRequest.OauthToken = token;
            return getRequest.Execute();
        }
    }
}
