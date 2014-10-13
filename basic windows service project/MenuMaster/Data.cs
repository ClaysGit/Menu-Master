using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace MenuMaster
{
    // This class represents one day of meal planning. Ex: Tuesday, September 23rd 2014.
    class Day
    {
        Guid ID { get; set; }
        Guid DayType { get; set; }
        List<Meal> Meals { get; set; }
    }
   
    // This class represents an individual meal. Ex: BLT on Tuesday, September 23rd, at 12:30pm
    class Meal
    {
        Guid ID { get; set; }
        Guid TypeID { get; set; } /* This points to a particular meal type, of which this individual
                                   * meal is an instance. This will inform us of the meal name
                                   * ("Lunch"), and meal time ("12:30pm") */
        Guid RecipeID { get; set; }
    }

    // This class represents an individual food item. Ex: BLT
    class Recipe
    {
        Guid ID { get; set; }
        string Name { get; set; }
        List<string> Ingredients { get; set; }
    }

    /*This class represents an individual ingredient used in a particular recipe. Will be used in 
     * future to calculate nutritional information, to automatically find substitutes, or things 
     * like that. Holding off for now until the rest of the framework comes together.
     * Ex: Bacon; Lettuce; Tomato
    class Ingredient
    {

    }
    */
}
