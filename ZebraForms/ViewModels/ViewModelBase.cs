using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZebraForms.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        string _title;

        public string Title {
            get { return _title; }
            set { SetField(ref _title, value); }
        }
    
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            //System.Diagnostics.Debug.WriteLine("Set Value For {0} to {1}", propertyName, value);

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
