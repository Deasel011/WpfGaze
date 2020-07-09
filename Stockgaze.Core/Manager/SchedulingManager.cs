using System;
using ASquare.WindowsTaskScheduler;
using ASquare.WindowsTaskScheduler.Models;

namespace Stockgaze.Core.Manager
{

    public class SchedulingManager
    {

        public void Schedule(Schedule schedule)
        {
            var task = WindowTaskScheduler.Configure();
                task.DeleteTask(schedule.TaskName,$"{Environment.CurrentDirectory}\\ScheduledSynchonizer.dll -e {schedule.SynchronizationName}");
                task.CreateTask("","").RunDaily().;
        }
        

    }

    public class Schedule
    {
        
    }

}