using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using Stockgaze.Core.Manager;
using Stockgaze.Core.Scheduling;
using Stockgaze.Core.WPFTools;

namespace Stockgaze.Core.Synchronization
{

    public class SynchronizationUCVM : BindableBase
    {

        private bool m_isRefreshing;

        private QuestradeOptionManager m_optionManager;

        private QuestradeSymbolDataManager m_symbolDataManager;

        private QuestradeSymbolIdManager m_symbolIdManager;

        private SchedulingManager m_schedulingManager;

        public bool IsRefreshing
        {
            get => m_isRefreshing;
            set => SetProperty(ref m_isRefreshing, value);
        }

        public QuestradeSymbolIdManager SymbolIdManager
        {
            get => m_symbolIdManager;
            set => SetProperty(ref m_symbolIdManager, value);
        }

        public QuestradeSymbolDataManager SymbolDataManager
        {
            get => m_symbolDataManager;
            set => SetProperty(ref m_symbolDataManager, value);
        }

        public SchedulingManager SchedulingManager
        {
            get => m_schedulingManager;
            set => SetProperty(ref m_schedulingManager, value);
        }

        public QuestradeOptionManager OptionManager
        {
            get => m_optionManager;
            set => SetProperty(ref m_optionManager, value);
        }

        public BindableCollection<InteractiveSchedule> Schedules { get; set; }

        public SynchronizationUCVM(QuestradeSymbolDataManager symbolDataManager, QuestradeSymbolIdManager symbolIdManager, QuestradeOptionManager optionManager, SchedulingManager schedulingManager)
        {
            m_symbolDataManager = symbolDataManager;
            m_symbolIdManager = symbolIdManager;
            m_schedulingManager = schedulingManager;
            m_optionManager = optionManager;
            Schedules = new BindableCollection<InteractiveSchedule>(schedulingManager.GetSchedules().Select(s=>new InteractiveSchedule(s)));
        }

        public void SaveSchedule(InteractiveSchedule schedule)
        {
            m_schedulingManager.Schedule(schedule.GetSchedule());
            Schedules.Add(schedule);
        }

    }

}