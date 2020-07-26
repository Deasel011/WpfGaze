// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Collections.Generic;
using Stockgaze.Core.Enums;
using Stockgaze.Core.Scheduling;
using Xunit;

namespace GazerTests
{

    public class ScheduleTest
    {

        [Fact]
        public void CreateSchedule()
        {
            var schedule = new Schedule();
            schedule.Description = "TestJob";
            schedule.Triggers.Add(new Schedule.Occurence{Recurrence = Schedule.RecurrenceType.Monthly, StartTime = new Schedule.ScheduleTime{Hour = 8}});
            schedule.SynchronizationType = TaskType.SymbolIds;
            schedule.TaskName = "CreateTestJob";
        }

    }

}