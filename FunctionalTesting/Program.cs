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
            timer.Start(0, 0, 10);
            Console.WriteLine("Waiting...");
            int wait = 5;
            while (wait > 0)
            {
                Thread.Sleep(1000);
                wait--;
            }
            Console.WriteLine("Pausing 2 seconds");
            timer.Pause();
            Thread.Sleep(2000);
            timer.Start();
            Console.WriteLine("Waiting again...");
            while (timer.Running)
            {
                Thread.Sleep(50);
            }
            Console.WriteLine("Done, press any key to exit");
            Console.ReadKey(true);
        }
    }
}
