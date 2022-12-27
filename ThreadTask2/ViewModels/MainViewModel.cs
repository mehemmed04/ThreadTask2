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
using ThreadTask2.Services;

namespace ThreadTask2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public void StartCoding()
        {
            while (true)
            {
                if (Values.Count > 0)
                {
                    for (int i = 0; i < Values.Count; i++)
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                        {
                            string CryptedData = CryptographyService.CodingToBase64(Values[i]);
                            Keys.Add(CryptedData);
                            Values.RemoveAt(i);
                            i -= 1;
                        });
                        Thread.Sleep(1000);
                    }
                }
            }
        }
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

        public bool IsSuspended { get; set; }

        public MainViewModel()
        {
            IsSuspended = false;
            Keys = new ObservableCollection<string>();
            Values = new ObservableCollection<string>();

            AddValueCommand = new RelayCommand((o) =>
            {
                Values.Add(NewValue);
                NewValue = String.Empty;
            });

            PlayCommand = new RelayCommand((o) =>
            {
                thread.Start();
            },
            (p) =>
            {
                return !thread.IsAlive;
            });
            PauseCommand = new RelayCommand((o) =>
            {
                thread.Suspend();
                IsSuspended = true;
            },
            (p) =>
            {
                return !IsSuspended;
            });
            ResumeCommand = new RelayCommand((o) =>
            {
                thread.Resume();
                IsSuspended = false;
            },
            (p) =>
            {
                return IsSuspended;
            });

            StopCommand = new RelayCommand((o) =>
            {
                thread.Abort();
            },
            (p) =>
            {
                return thread.IsAlive && !IsSuspended;
            });
            thread = new Thread(() => { StartCoding(); });
        }


    }
}
