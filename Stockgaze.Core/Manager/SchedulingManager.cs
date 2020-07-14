using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.TaskScheduler;
using Stockgaze.Core.Scheduling;

namespace Stockgaze.Core.Manager
{

    public class SchedulingManager
    {

        private const string TASKPREFIX = "Stockgaze-";

        private TaskService TaskService { get; }

        private static DaysOfTheWeek BusinessDays => DaysOfTheWeek.Monday | DaysOfTheWeek.Tuesday | DaysOfTheWeek.Wednesday | DaysOfTheWeek.Thursday |
                                                     DaysOfTheWeek.Friday;

        public SchedulingManager()
        {
            TaskService = new TaskService();
        }

        public List<Schedule> GetSchedules()
        {
            return TaskService.AllTasks.Where(t => t.Name.StartsWith(TASKPREFIX)).Select(t => new Schedule()).ToList();
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
                case Scheduling.Schedule.RecurrenceType.Daily:
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
                case Scheduling.Schedule.RecurrenceType.Weekly:
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
                case Scheduling.Schedule.RecurrenceType.Monthly:
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

    }

}