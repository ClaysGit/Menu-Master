using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

using GoogleCalendarController;

using System.IO;
using System.Diagnostics;

namespace MenuMaster
{
    public class User
    {
        private string name;
        private long ID;
        private string email;
        private string password;

        private MealWeek mealPlanningWeek;
        private List<Recipe> recipes;
        private Dictionary<string, byte> dayTypes;
        private Dictionary<string, byte> mealTypes;

        public googleCalendarController CalendarController;

        private string planCalendarID;

        public User()
        {
            CalendarController = new googleCalendarController();

            if (!(load()))
            {
                planCalendarID = "";
                Debug.WriteLine("Failed to load!");
            }
            CalendarList calList = CalendarController.listCalendars();

            if (!(calList.Items.Any(x => x.Id == planCalendarID)))
            {
                Calendar PlanCalendar = CalendarController.newCalendar(Globals.MenuMasterPlanCalendarSummary, "GMT");
                CalendarListEntry PlanCalendarListEntry = CalendarController.addCalendar(PlanCalendar);
                planCalendarID = PlanCalendarListEntry.Id;
                Debug.WriteLine("Calendar ID = " + planCalendarID);

            }

            save();

        }

        // A dummy method, to show our program is properly communicating with Google.
        public void addHourlyEvent()
        {
            DateTime time = DateTime.Now;

            CalendarController.newEvent("MenuMaster Hourly Test Event", "0,0,0", planCalendarID,
                time, time.AddHours(1));
        }

        // Save user data (currently just the ID of the MenuMaster meal calendar) in a plaintext file
        public void save()
        {
            File.WriteAllText("userdata.txt", planCalendarID);
        }

        // Load user data (currently just the ID of the MenuMaster meal calendar) from a plaintext file. The ID is
        // the unique identifier given by Google when the calendar was created. If we didn't save it, we'd have to
        // search for it every time the program started up, which takes a bit of calculation and several REST requests
        public bool load()
        {
            if (File.Exists("userdata.txt"))
            {
                planCalendarID = File.ReadAllText("userdata.txt");
                return true;
            }
            else
                return false;
        }

    }

    static partial class Globals
    {
        public const string MenuMasterPlanCalendarSummary = "MenuMaster main meal plan calendar";
    }
}

