﻿using System;
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
                baseSpan += _pauseSum;

                return baseSpan;
            }
        }

        public int UpdateDelay = 50;

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

        public void Start()
        {
            if (!Running)
            {
                _startTime = DateTime.Now;
                Running = true;
                if (_updateAction != null)
                {
                    new Thread(_updateWork).Start();
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
