using System;
using Prism.Mvvm;

namespace Stockgaze.Core.Helpers
{
    public class ProgressReporter: BindableBase
    {

        public ProgressReporter(int totalElements)
        {
            TotalElements = totalElements;
        }

        private ulong m_progress;

        private int TotalElements { get; set; }

        public ulong Progress
        {
            get => m_progress;
            set => SetProperty(ref m_progress, value);
        }

        public void SetProgress(int index)
        {
            try
            {
                Progress = Convert.ToUInt64(Math.Round((double)index / (double)TotalElements * 100));
            }
            catch (OverflowException e)
            {
                //SUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUP
            }
        }

        public void SetTotalElements(int count)
        {
            TotalElements = count;
        }
    }
}