//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Win32.TaskScheduler;
using Stockgaze.Core.Scheduling;

namespace Stockgaze.Core.Services
{

    public class SchedulingService
    {

        public bool Save(Schedule scheduleToSave)
        {
            return false;
        }

        public List<Schedule> GetSavedSchedules()
        {
            var stockgazeTasks = TaskService.Instance.AllTasks.Where(t => t.Name.StartsWith("Stockgaze-"));
            
            
            return null;
        }

    }

}