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
            /*
            CountdownTimer timer = new CountdownTimer(span =>
            {
                Console.Write(span.ToString(@"hh\:mm\:ss") + "\r");
            });
            */
            CountdownTimer timer = new CountdownTimer();
            Console.WriteLine("Starting timer");
            timer.Start(0, 0, 5);
            Console.WriteLine("Waiting...");
            int wait = 20;
            while (wait > 0)
            {
                if(!timer.Finished) Console.Write(timer.Remaining.ToString(@"hh\:mm\:ss") + "\r");
                else Console.Write("-" +timer.Remaining.ToString(@"hh\:mm\:ss") + "\r");
                Thread.Sleep(1000);
                wait--;
            }
            /*
            Console.WriteLine("Pausing 2 seconds");
            timer.Pause();
            Thread.Sleep(2000);
            timer.Start();
            Console.WriteLine("Waiting again...");
            while (timer.Running)
            {
                Thread.Sleep(50);
            }
            */
            Console.WriteLine("Done, press any key to exit");
            Console.ReadKey(true);
        }
    }
}
