//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace OptionGaze.WPFTools
{

    public class BindableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {

        public BindableCollection()
        {
        }

        public BindableCollection(IEnumerable<T> collection) : base(collection)
        {
        }

        public BindableCollection(List<T> list) : base(list)
        {
        }

        public event EventHandler<EventArgs> ItemChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            ItemChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ItemChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void ClearItems()
        {
            Items.ToList().ForEach(item => item.PropertyChanged -= OnItemPropertyChanged);
            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            item.PropertyChanged += OnItemPropertyChanged;
        }

        protected override void RemoveItem(int index)
        {
            this[index].PropertyChanged -= OnItemPropertyChanged;
            base.RemoveItem(index);
        }

    }

}