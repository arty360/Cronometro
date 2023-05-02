using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Cronometro.ViewModel
{
    public class CronometroViewModel: ViewModelBase
    {
        const string _startingTime = "00:00:00";
        Timer _timer;
        Stopwatch _stopWatch;        

        public CronometroViewModel()
        {
            _stopWatch = new Stopwatch();
            _timer = new Timer(interval: 1000);
            TimeDisplay = _startingTime;

            _timer.Elapsed += OnTimerElapsed;
        }

        #region Properties

        string _timeDisplay;
        public string TimeDisplay
        {
            get
            {
                return _timeDisplay;
            }
            set
            {
                if (_timeDisplay != value)
                {
                    _timeDisplay = value;
                    OnPropertyChanged(nameof(TimeDisplay));
                }
            }
        }

        bool _startIsEnabled = true;
        public bool StartIsEnabled
        {
            get
            {
                return _startIsEnabled;
            }
            set
            {
                if (_startIsEnabled != value)
                {
                    _startIsEnabled = value;
                    OnPropertyChanged(nameof(StartIsEnabled));
                }
            }
        }

        bool _pauseIsEnabled = false;
        public bool PauseIsEnabled
        {
            get
            {
                return _pauseIsEnabled;
            }
            set
            {
                if (_pauseIsEnabled != value)
                {
                    _pauseIsEnabled = value;
                    OnPropertyChanged(nameof(PauseIsEnabled));
                }
            }
        }

        bool _stopIsEnabled = false;
        public bool StopIsEnabled
        {
            get
            {
                return _stopIsEnabled;
            }
            set
            {
                if (_stopIsEnabled != value)
                {
                    _stopIsEnabled = value;
                    OnPropertyChanged(nameof(StopIsEnabled));
                }
            }
        }

        #endregion Properties

        #region Commands

        private ViewModelCommand startCommand;
        public ICommand StartCommand
        {
            get
            {
                if (startCommand == null)
                {
                    startCommand = new ViewModelCommand(Start);
                }
                return startCommand;
            }
        }

        private ViewModelCommand pauseCommand;
        public ICommand PauseCommand
        {
            get
            {
                if (pauseCommand == null)
                {
                    pauseCommand = new ViewModelCommand(Pause);
                }
                return pauseCommand;
            }
        }

        private ViewModelCommand stopCommand;
        public ICommand StopCommand
        {
            get
            {
                if (stopCommand == null)
                {
                    stopCommand = new ViewModelCommand(Stop);
                }
                return stopCommand;
            }
        }        

        #endregion Commands

        private void Start(object obj)
        {
            _stopWatch.Start();
            _timer.Start();
            StartIsEnabled = false;
            PauseIsEnabled = StopIsEnabled = !StartIsEnabled;
        }

        private bool CanExecuteStartCommand(object obj)
        {
            return !_stopWatch.IsRunning;
        }

        private void Pause(object obj)
        {
            _stopWatch.Stop();
            _timer.Stop();
            StartIsEnabled = true;
            PauseIsEnabled = false;
            StopIsEnabled = true;
        }

        private bool CanExecutePauseCommand(object obj)
        {
            return _stopWatch.IsRunning;
        }

        private void Stop(object obj)
        {
            _stopWatch.Reset();
            _timer.Close();
            TimeDisplay = _startingTime;
            StartIsEnabled = true;
            PauseIsEnabled = false;
            StopIsEnabled = false;
        }

        private bool CanExecuteStopCommand(object obj)
        {
            return _stopWatch.IsRunning;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render,() =>
            {
                TimeDisplay = _stopWatch.Elapsed.ToString(format: @"hh\:mm\:ss");
            });
        }
    }
}