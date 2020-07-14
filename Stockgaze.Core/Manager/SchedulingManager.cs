using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.TaskScheduler;
using Stockgaze.Core.Enums;


namespace Stockgaze.Core.Manager
{

    public class SchedulingManager
    {

        private TaskService TaskService { get; set; }
        
        public SchedulingManager()
        {
            TaskService = new TaskService();
        }

        private const string TASKPREFIX = "Stockgaze-";
        public List<Schedule> GetSchedules()
        {
            return TaskService.AllTasks.Where(t => t.Name.StartsWith(TASKPREFIX)).Select(t=> new Schedule()).ToList();
        }
        public void Schedule(Schedule schedule)
        {
            if (GetSchedules().Any(t => t.TaskName.Equals($"{TASKPREFIX}{schedule.TaskName}")))
            {
                throw new ArgumentException($"A task name {TASKPREFIX}{schedule.TaskName} already exists.");
            }
            var taskDefinition = TaskService.Instance.NewTask();

            taskDefinition.RegistrationInfo.Description = schedule.Description;
            taskDefinition.Triggers.AddRange(schedule.Triggers.Select(OccurenceToTrigger));
            taskDefinition.Actions.Add($"{Environment.CurrentDirectory}\\ScheduledSynchonizer.dll", $"-e {schedule.SynchronizationType}");
            taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;
            TaskService.Instance.RootFolder.RegisterTaskDefinition($"{TASKPREFIX}{schedule.TaskName}", taskDefinition);

        }

        public Trigger OccurenceToTrigger(Schedule.Occurence occurence)
        {
            switch (occurence.Recurrence)
            {
                case Manager.Schedule.RecurrenceType.Daily:
                    var dailyTrigger = new WeeklyTrigger();
                    var startTime = DateTime.Today;
                    startTime = startTime.AddHours(occurence.StartTime.Hour);
                    startTime = startTime.AddMinutes(occurence.StartTime.Minute);
                    startTime = startTime.AddSeconds(occurence.StartTime.Second);
                    dailyTrigger.StartBoundary = startTime;
                    dailyTrigger.DaysOfWeek = BusinessDays;
                    dailyTrigger.WeeksInterval = 1;
                    return dailyTrigger;
                    break;
                case Manager.Schedule.RecurrenceType.Weekly:
                    var weeklyTrigger = new WeeklyTrigger();
                    var weeklyStartTime = DateTime.Today;
                    weeklyStartTime = weeklyStartTime.AddHours(occurence.StartTime.Hour);
                    weeklyStartTime = weeklyStartTime.AddMinutes(occurence.StartTime.Minute);
                    weeklyStartTime = weeklyStartTime.AddSeconds(occurence.StartTime.Second);
                    weeklyTrigger.StartBoundary = weeklyStartTime;
                    weeklyTrigger.DaysOfWeek = DaysOfTheWeek.Monday;
                    weeklyTrigger.WeeksInterval = 1;
                    return weeklyTrigger;
                    break;
                case Manager.Schedule.RecurrenceType.Monthly:
                    var monthlyTrigger = new MonthlyTrigger();
                    var monthlyStartTime = DateTime.Today;
                    monthlyStartTime = monthlyStartTime.AddHours(occurence.StartTime.Hour);
                    monthlyStartTime = monthlyStartTime.AddMinutes(occurence.StartTime.Minute);
                    monthlyStartTime = monthlyStartTime.AddSeconds(occurence.StartTime.Second);
                    monthlyTrigger.StartBoundary = monthlyStartTime;
                    monthlyTrigger.DaysOfMonth = new[] {1};
                    return monthlyTrigger;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static DaysOfTheWeek BusinessDays => DaysOfTheWeek.Monday | DaysOfTheWeek.Tuesday | DaysOfTheWeek.Wednesday | DaysOfTheWeek.Thursday |
                                                     DaysOfTheWeek.Friday;

    }

    public class Schedule
    {
        public class ScheduleTime
        {
            public int Hour { get; set; }
            public int Minute { get; set; }
            public int Second { get; set; }

        }

        public enum RecurrenceType
        {
            Daily,
            Weekly,
            Monthly
        }
        
        public List<Occurence> Triggers { get; set; }

        public TaskType SynchronizationType { get; set; }

        public string TaskName { get; set; }

        public string Description { get; set; }
        
        
        public class Occurence
        {
            public ScheduleTime StartTime { get; set; }
        
            public RecurrenceType Recurrence { get; set; }
        }

    }

}