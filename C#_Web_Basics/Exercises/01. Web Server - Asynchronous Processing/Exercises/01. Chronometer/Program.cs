using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace _01._Chronometer
{
    class Program
    {
        static void Main(string[] args)
        {
            IChronometer chronometer = new Chronometer();
            var action = Console.ReadLine();

            while (action != "exit")
            {
                switch (action)
                {
                    case "start":
                        chronometer.Start();
                        break;
                    case "stop":
                        chronometer.Stop();
                        break;
                    case "lap":
                        Console.WriteLine(chronometer.Lap());
                        break;
                    case "laps":
                        Console.WriteLine("Laps:" + (chronometer.Laps.Count == 0
                            ? " no laps" 
                            : Environment.NewLine 
                              + String.Join(Environment.NewLine, chronometer
                                  .Laps
                                  .Select((lap, index) => $"{index}. {lap}"))));
                        break;
                    case "time":
                        Console.WriteLine(chronometer.GetTime);
                        break;
                    case "reset":
                        chronometer.Reset();
                        break;
                }

                action = Console.ReadLine();
            }
        }
    }
}
