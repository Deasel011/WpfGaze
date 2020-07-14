using System.Collections.Generic;
using Prism.Mvvm;
using Stockgaze.Core.Enums;

namespace Stockgaze.Core.Scheduling
{

    public class InteractiveSchedule : BindableBase
    {

        public Schedule GetSchedule()
        {
            return m_schedule;
        } 

        private readonly Schedule m_schedule;

        public InteractiveSchedule()
        {
            m_schedule = new Schedule();
        }

        public InteractiveSchedule(Schedule schedule)
        {
            m_schedule = schedule;
        }
        
        public TaskType SynchronizationType
        {
            get => m_schedule.SynchronizationType;
            set
            {
                m_schedule.SynchronizationType = value;
                RaisePropertyChanged(nameof(SynchronizationType));
            }
        }

        public string TaskName
        {
            get => m_schedule.TaskName;
            set
            {
                m_schedule.TaskName = value;
                RaisePropertyChanged(nameof(TaskName));
            }
        }

        public string Description
        {
            get => m_schedule.Description;
            set
            {
                m_schedule.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }


        public void AddTrigger(Schedule.Occurence occurence)
        {
            Triggers.Add(occurence);
            RaisePropertyChanged(nameof(Triggers));
        }

        public void RemoveTrigger(Schedule.Occurence occurence)
        {
            Triggers.Remove(occurence);
            RaisePropertyChanged(nameof(Triggers));
        }

        public List<Schedule.Occurence> Triggers => m_schedule.Triggers;

    }

}