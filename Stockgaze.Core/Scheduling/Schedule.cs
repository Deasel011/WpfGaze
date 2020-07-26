using System.Collections.Generic;
using Stockgaze.Core.Enums;

namespace Stockgaze.Core.Scheduling
{

    public class Schedule
    {

        public List<Occurence> Triggers { get; set; }

        public TaskType SynchronizationType { get; set; }

        private string _taskName;
        public string TaskName
        {
            get => _taskName;
            set => _taskName = $"Stockgaze-{value}";
        }

        public string Description { get; set; }

        public enum RecurrenceType
        {

            Daily,

            Weekly,

            Monthly

        }

        public class ScheduleTime
        {

            private sealed class HourMinuteSecondEqualityComparer : IEqualityComparer<ScheduleTime>
            {

                public bool Equals(ScheduleTime x, ScheduleTime y)
                {
                    if (ReferenceEquals(x, y)) return true;
                    if (ReferenceEquals(x, null)) return false;
                    if (ReferenceEquals(y, null)) return false;
                    if (x.GetType() != y.GetType()) return false;
                    return x.Hour == y.Hour && x.Minute == y.Minute && x.Second == y.Second;
                }

                public int GetHashCode(ScheduleTime obj)
                {
                    unchecked
                    {
                        var hashCode = obj.Hour;
                        hashCode = (hashCode * 397) ^ obj.Minute;
                        hashCode = (hashCode * 397) ^ obj.Second;
                        return hashCode;
                    }
                }

            }

            public static IEqualityComparer<ScheduleTime> HourMinuteSecondComparer { get; } = new HourMinuteSecondEqualityComparer();

            public int Hour { get; set; }

            public int Minute { get; set; }

            public int Second { get; set; }

        }


        public class Occurence
        {

            public ScheduleTime StartTime { get; set; }

            public RecurrenceType Recurrence { get; set; }

        }

        public Schedule()
        {
            Triggers = new List<Occurence>();
        }

    }

}