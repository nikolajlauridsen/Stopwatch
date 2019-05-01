using System;
using System.Text;
using System.Threading;

namespace Timers
{
    public class StopWatch
    {
        public TimeSpan Elapsed
        {
            get
            {
                TimeSpan baseSpan = new TimeSpan(0);
                if (Running) baseSpan = DateTime.Now.Subtract(_startTime);
                baseSpan += _pauseSum;

                return baseSpan;
            }
        }

        public int UpdateDelay = 50;
        public string FormatString = @"hh\:mm\:ss\.ff";

        public bool Running { get; private set; }
        private DateTime _startTime;
        private TimeSpan _pauseSum = new TimeSpan(0);
        private Action<TimeSpan> _updateAction = null;


        public StopWatch(Action<TimeSpan> UpdateAction)
        {
            _updateAction = UpdateAction;
        }

        public StopWatch() : this(null)
        {

        }

        public void SetUpdateEvent(Action<TimeSpan> updateAction)
        {
            _updateAction = updateAction;
        }

        public void Start()
        {
            if (!Running)
            {
                _startTime = DateTime.Now;
                Running = true;
                if (_updateAction != null)
                {
                    Thread t1 = new Thread(_updateWork);
                    t1.IsBackground = true;
                    t1.Start();
                }
            }
        }

        public void Reset()
        {
            Running = false;
            _pauseSum = new TimeSpan(0);
            _updateAction?.Invoke(new TimeSpan(0));
        }

        public void Pause()
        {
            if (Running)
            {
                Running = false;
                _pauseSum += DateTime.Now.Subtract(_startTime);
                _updateAction?.Invoke(Elapsed);
            }
        }

        public void SetMilisecondDigits(int digits)
        {
            if (digits == 0) {
                FormatString = @"hh\:mm\:ss";
            } else {
                StringBuilder formatBuilder = new StringBuilder(@"hh\:mm\:ss\.");
                for (int i = 0; i < digits; i++) {
                    formatBuilder.Append("f");
                }

                FormatString = formatBuilder.ToString();
            }
        }

        private void _updateWork()
        {
            while (Running)
            {
                _updateAction(Elapsed);
                Thread.Sleep(UpdateDelay);
            }

        }

    }
}
