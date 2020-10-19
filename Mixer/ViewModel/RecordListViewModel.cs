using Mixer.Model;
using Mixer.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mixer.ViewModel
{
    internal class RecordListViewModel : INotifyPropertyChanged
    {
        private List<RecordViewModel> RecordViews;

        public ObservableCollection<RecordViewModel> RecordViewsObservable => new ObservableCollection<RecordViewModel>(RecordViews);

        private List<RecordViewModel> RecordViewsBlended;

        public ObservableCollection<RecordViewModel> RecordViewsBlendedObservable => new ObservableCollection<RecordViewModel>(RecordViewsBlended);

        private RelayCommand addRecordCommand;

        public RelayCommand AddRecordCommand
        {
            get => addRecordCommand;
        }

        private RelayCommand copyToClipboardRecordViews;

        public RelayCommand CopyToClipboardRecordViews
        {
            get => copyToClipboardRecordViews;
        }

        private RelayCommand copyToClipboardRecordViewsBlended;

        public RelayCommand CopyToClipboardRecordViewsBlended
        {
            get => copyToClipboardRecordViewsBlended;
        }

        private RelayCommand blendCommand;

        public RelayCommand BlendCommand
        {
            get => blendCommand;
        }

        private bool canExecute;

        public RecordListViewModel()
        {
            RecordViews = new List<RecordViewModel>();
            RecordViewsBlended = new List<RecordViewModel>();
            addRecordCommand = new RelayCommand(AddRecord, CanExecute);
            copyToClipboardRecordViews = new RelayCommand((o) => { CopyToClipboard(RecordViews); }, CanExecute);
            copyToClipboardRecordViewsBlended = new RelayCommand((o) => { CopyToClipboard(RecordViewsBlended); }, CanExecute);
            blendCommand = new RelayCommand(Blend, CanExecute);
            canExecute = true;
        }

        public bool CanExecute(object o)
        {
            return canExecute;
        }

        public void AddRecord(object obj = null)
        {
            RecordViews.Add(new RecordViewModel(new Record(), Remove, CanExecute));
            OnPropertyChanged("RecordViewsObservable");
        }

        public void Remove(RecordViewModel rvm)
        {
            RecordViews.Remove(rvm);
            OnPropertyChanged("RecordViewsObservable");
        }

        public void RemoveFromBlended(RecordViewModel rvm)
        {
            RecordViewsBlended.Remove(rvm);
            OnPropertyChanged("RecordViewsBlendedObservable");
        }

        public async void Blend(object o)
        {
            canExecute = false;
            var list = RecordViews.Select(r => r.GetSourse).ToArray();
            var newList = new Dictionary<int, Record>();

            var random = new Random();

            int rIndex = 0;

            await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < list.Length; i++)
                {
                    do
                    {
                        rIndex = random.Next(0, list.Length);
                        if (newList.Count == list.Length -1 && rIndex == i)
                        {
                            i = 0;
                            newList = new Dictionary<int, Record>();
                        }
                    } while (rIndex == i || newList.ContainsKey(rIndex));

                    newList.Add(rIndex, new Record { Key = list[i].Key, Value = list[rIndex].Value });
                }
            });

            RecordViewsBlended = new List<RecordViewModel>();
            foreach (var item in newList.Values)
            {
                RecordViewsBlended.Add(new RecordViewModel(item, RemoveFromBlended, CanExecute));
            }
            canExecute = true;
            OnPropertyChanged("RecordViewsBlendedObservable");
            
        }

        public void CopyToClipboard(List<RecordViewModel> rvm)
        {
            canExecute = false;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var rv in rvm)
            {
                var r = rv.GetSourse;
                stringBuilder.Append($"{r.Key}\t{r.Value}\n");
            }
            Clipboard.SetText(stringBuilder.ToString());
            canExecute = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
