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

        public bool Running { get; private set; }

        private DateTime _startTime;
        private DateTime _endTime;
        public TimeSpan Duration { get; private set; }
        private Action<TimeSpan> _updateAction;

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


        private void _updateWork()
        {
            while (Running)
            {
                _updateAction?.Invoke(Remaining);
                if (DateTime.Now >= _endTime) Running = false;
                Thread.Sleep(UpdateDelay);
            }
        }
    }
}
