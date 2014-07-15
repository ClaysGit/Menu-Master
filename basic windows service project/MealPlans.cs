using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMaster
{
    //This represents the largest period of time that the program will "look ahead" when constructing a meal plan.
    //Should typically represent a week's worth of planning.
    class MealWeek
    {
        private int length;
        private List<MealDay> days;

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public List<MealDay> Days
        {
            get { return days; }
            set { days = value; }
        }

    }

    //Represents one days worth of meal planning. Each day has a type - typically weekend/weekday, holiday, etc. It
    //also contains a number of meals. 
    class MealDay
    {

    }

    class Meal
    {

    }

    class DayType
    { 
    }

    class MealType
    {

    }
}
