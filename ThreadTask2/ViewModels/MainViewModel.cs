using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ThreadTask2.Commands;

namespace ThreadTask2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public Thread thread { get; set; }
        public DispatcherTimer timer { get; set; }
        private ObservableCollection<string> keys;

        public ObservableCollection<string> Keys
        {
            get { return keys; }
            set { keys = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> values;

        public ObservableCollection<string> Values
        {
            get { return values; }
            set { values = value; OnPropertyChanged(); }
        }

        private string newValue;

        public string NewValue
        {
            get { return newValue; }
            set { newValue = value; OnPropertyChanged(); }
        }

        public RelayCommand AddValueCommand { get; set; }
        public RelayCommand PlayCommand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public RelayCommand ResumeCommand { get; set; }



        public MainViewModel()
        {
            thread = new Thread(() =>
            {
                timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                timer.Tick += StartCoding;
                timer.Start();
            });
            thread.Start();
            Keys = new ObservableCollection<string>();
            Values = new ObservableCollection<string>();

            AddValueCommand = new RelayCommand((o) =>
            {
                Values.Add(NewValue);
            });

            PlayCommand = new RelayCommand((o) =>
            {
                timer.Start();
            },
            (p) =>
            {
                return !timer.IsEnabled;
            });




            StopCommand = new RelayCommand((o) =>
            {
                timer.Stop();
            },
            (p) =>
            {
                return timer.IsEnabled;
            });



        }

        private void StartCoding(object sender, EventArgs e)
        {
            if (Values.Count > 0)
            {
                for (int i = 0; i < Values.Count; i++)
                {
                    Keys.Add(Values[i]);
                    Values.RemoveAt(i);
                    i -= 1;
                    Thread.Sleep(1000);
                }
                //foreach (var item in Values)
                //{
                //    Keys.Add(item);
                //    Values.Remove(item);
                //    Thread.Sleep(1000);
                //}
            }
        }
    }
}
