﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stopwatch.Interfaces
{
    public interface SettingsPublisher
    {
        void AddSubscriber(SettingsSubscriber subsriber);
        void NotifySubscribers();
    }
}
