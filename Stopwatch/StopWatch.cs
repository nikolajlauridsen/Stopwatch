using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stopwatch
{
    class StopWatch
    {
        public TimeSpan Elapsed
        {
            get
            {
                TimeSpan baseSpan = DateTime.Now.Subtract(_startTime);
                foreach (TimeSpan span in _pauses)
                {
                    baseSpan += span;
                }

                return baseSpan;
            }
        }

        public int UpdateDelay = 50;

        private bool _running = false;
        private DateTime _startTime;
        private List<TimeSpan> _pauses = new List<TimeSpan>();
        private Action<TimeSpan> _updateAction = null;


        public StopWatch(Action<TimeSpan> UpdateAction)
        {
            _updateAction = UpdateAction;
        }

        public StopWatch() : this(null)
        {

        }

        public void Start()
        {
            if (!_running)
            {
                _startTime = DateTime.Now;
                _running = true;
                if (_updateAction != null)
                {
                    new Thread(_updateWork).Start();
                }
            }
        }

        public void Stop()
        {
            _running = false;
            _pauses.Clear();
        }

        public void Pause()
        {
            _running = false;
            _pauses.Add(DateTime.Now.Subtract(_startTime));
        }

        private void _updateWork()
        {
            while (_running)
            {
                _updateAction(Elapsed);
                Thread.Sleep(UpdateDelay);
            }
        }


    }
}
