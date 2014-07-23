using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMaster
{
    class User
    {
        private string name;
        private long ID;
        private string email;
        private string password;

        private MealWeek mealPlanningWeek;
        private List<Recipe> recipes;
        private Dictionary<string, byte> dayTypes;
        private Dictionary<string, byte> mealTypes;
    }
}
