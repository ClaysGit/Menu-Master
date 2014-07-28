using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using GoogleCalendarController;

using Google.Apis.Calendar.v3;

namespace MenuMaster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new MenuMaster() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
