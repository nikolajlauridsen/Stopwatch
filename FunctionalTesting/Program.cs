using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Timers;

namespace FunctionalTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating timer");
            CountdownTimer timer = new CountdownTimer(span =>
            {
                Console.Write(span.ToString(@"hh\:mm\:ss") + "\r");
            });
            Console.WriteLine("Starting timer");
            timer.Start(0, 0, 5);
            Console.WriteLine("Waiting...");
            while (timer.Running)
            {
                Thread.Sleep(50);
            }

            Console.WriteLine("Done, press any key to exit");
            Console.ReadKey(true);
        }
    }
}
