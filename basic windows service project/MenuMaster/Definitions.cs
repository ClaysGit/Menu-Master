using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

using System.IO;

namespace MenuMaster
{
    // This class represents a type of day. Ex: Work days; Weekends; BLT Tuesdays
    class DayType
    {
        Guid ID { get; set; }
        string Name { get; set; }

        List<Guid> MealTypeIDs { get; set; } /* This is a list of meals for this particular day.
                                              * Ex: Breakfast; Lunch; Dinner */
    }

    // This class represents a meal category. Ex: Lunch
    class MealType
    {
        Guid ID { get; set; }
        string Name { get; set; }
        List<Guid> RecipeOptions { get; set; }
        DateTime Time { get; set; }
    }

    // This class instantiates and contains our definitions, including a list of meal types and a
    // list of day types
    class Definitions
    {
        private Definitions()
        {
            DayTypes = new List<DayType>();
            MealTypes = new List<MealType>();
        }

        public void Save()
        {
            var serializer = new XmlSerializer(GetType());

            using (var fs = File.OpenWrite(Globals.DefinitionsPath))
            {
                serializer.Serialize(fs, this);
            }
        }

        List<DayType> DayTypes { get; set; }
        List<MealType> MealTypes { get; set; }
    }
}
