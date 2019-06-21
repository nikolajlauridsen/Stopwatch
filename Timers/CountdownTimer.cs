using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Timers
{
    public class CountdownTimer
    {
        public TimeSpan Remaining
        {
            get { return _endTime.Subtract(DateTime.Now); }
        }

        public int UpdateDelay = 50;

        private object _runningLock = new object();
        public bool Running
        {
            get
            {
                lock (_runningLock)
                {
                    return _running;
                }
            }
            private set
            {
                lock (_runningLock)
                {
                    _running = value;
                }
            }
        }

        private bool _running;

        public bool Finished
        {
            get
            {
                if (DateTime.Now >= _endTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool AutoStop = true;

        private DateTime _startTime;
        private DateTime _endTime;
        public TimeSpan Duration { get; private set; }
        private Action<TimeSpan> _updateAction;
        private Action _onFinish;

        public CountdownTimer(Action<TimeSpan> updateAction)
        {
            _updateAction = updateAction;
        }

        public CountdownTimer() : this(null)
        {
        }

        public void SetDuration(int hours, int minutes, int seconds)
        {
            Duration = new TimeSpan(hours, minutes, seconds);
        }

        public void SetUpdateAction(Action<TimeSpan> updateAction)
        {
            _updateAction = updateAction;
        }

        public void SetOnFinishedAction(Action finishAction)
        {
            _onFinish = finishAction;
        }


        public void SetDuration(TimeSpan duration)
        {
            Duration = duration;
        }

        public void Start()
        {
            _startTime = DateTime.Now;
            _endTime = _startTime.Add(Duration);
            Running = true;
            if (_updateAction != null)
            {
                Thread updateThread = new Thread(_updateWork);
                updateThread.IsBackground = true;
                updateThread.Start();
            } else if (AutoStop)
            {
                Thread finishWatcher = new Thread(_finishWatcherWork);
                finishWatcher.IsBackground = true;
                finishWatcher.Start();
            }
        }

        public void Start(int hours, int minutes, int seconds)
        {
            SetDuration(hours, minutes, seconds);
            Start();
        }

        public void Start(TimeSpan duration)
        {
            SetDuration(Duration);
            Start();
        }

        public void Pause()
        {
            Running = false;
            Duration = Remaining;
        }

        private void _updateWork()
        {
            while (Running)
            {
                _updateAction?.Invoke(Remaining);
                if (DateTime.Now >= _endTime && AutoStop)
                {
                    Running = false;
                }
                Thread.Sleep(UpdateDelay);
            }
            _onFinish?.Invoke();
        }

        private void _finishWatcherWork()
        {
            while (Running) {
                Thread.Sleep(UpdateDelay);
            }

            _onFinish();
        }
    }
}
