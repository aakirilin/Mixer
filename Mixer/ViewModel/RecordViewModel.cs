using Mixer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mixer.ViewModel
{
    internal class RecordViewModel : INotifyPropertyChanged
    {
        private Record Sourse;

        public Record GetSourse
        {
            get => Sourse.Copy();
        }

        public string Key 
        {
            get => Sourse.Key;
            set 
            {
                Sourse.Key = value;
                OnPropertyChanged("Key");
            } 
        }

        public string Value
        {
            get => Sourse.Value;
            set
            {
                Sourse.Value = value;
                OnPropertyChanged("Value");
            }
        }

        public bool RecordEqals(Record another)
        {
            return Sourse == another;
        }

        private RelayCommand deleteCommand;

        public RelayCommand DeleteCommand
        {
            get => deleteCommand;
        }

        public RecordViewModel(Record sourse, Action<RecordViewModel> onDelete, Func<object, bool> canExecute)
        {
            this.Sourse = sourse;
            this.deleteCommand = new RelayCommand((o) => { onDelete(this); });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
