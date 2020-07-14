using System;
using Prism.Services.Dialogs;

namespace Stockgaze.Core.Scheduling
{

    public class SchedulingUCVM: IDialogAware
    {

        public bool CanCloseDialog()
        {
            throw new NotImplementedException();
        }

        public void OnDialogClosed()
        {
            throw new NotImplementedException();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            throw new NotImplementedException();
        }

        public string Title { get; }

        public event Action<IDialogResult> RequestClose;

    }

}