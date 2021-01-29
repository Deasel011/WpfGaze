using System.Windows.Controls;
using Stockgaze.Core.Scheduling;

namespace OptionGaze.Scheduling
{

    public partial class ScheduleDialogUserControl : UserControl
    {

        private readonly SchedulingUCVM m_schedulingUCVM;

        public ScheduleDialogUserControl()
        {
            InitializeComponent(); 
            m_schedulingUCVM = new SchedulingUCVM();
            DataContext = m_schedulingUCVM;
        }

    }

}