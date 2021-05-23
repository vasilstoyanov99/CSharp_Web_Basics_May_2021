using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01._Chronometer
{
    public class Chronometer : IChronometer
    {
        private long _milliseconds;
        private bool _isRunning;
        public string GetTime => $"{_milliseconds / 6000:D2}: {_milliseconds / 1000:D2}:" +
                                 $"{_milliseconds % 1000:D2}";

        public List<string> Laps { get; private set; }

        public Chronometer()
        {
            Reset();
        }

        public void Start()
        {
            _isRunning = true;

            Task.Run(() =>
            {
                while (_isRunning)
                {
                    Thread.Sleep(1);
                    _milliseconds++;
                }
            });
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public string Lap()
        {
            var lap = GetTime;
            Laps.Add(lap);
            return lap;
        }

        public void Reset()
        {
            Stop();
            _milliseconds = 0;
            Laps = new List<string>();
        }
    }
}
