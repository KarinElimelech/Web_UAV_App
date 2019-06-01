using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class TimeTracker
    {
        private static TimeTracker self = null;
        private DateTime startTime;
        private TimeSpan interval;
        private bool timePassed;
        private readonly static object mutex = new object();

        private TimeTracker()
        {
            timePassed = false;
            startTime = DateTime.MinValue;
        }

        public static TimeTracker Instance
        {
            get
            {
                lock (mutex)
                {
                    if (self == null)
                    {
                        self = new TimeTracker();
                    }
                    return self;
                }
            }
        }

        public double Interval
        {
            set
            {
                lock(mutex)
                {
                    if (timePassed)
                    {
                        timePassed = false;
                        startTime = DateTime.MinValue;
                    }
                    interval = TimeSpan.FromSeconds(value);
                }
            }
        }

        public void StartTimer()
        {
            lock(mutex)
            {
                if (startTime == DateTime.MinValue)
                {
                    timePassed = false;
                    startTime = DateTime.Now;
                }
            }
        }

        public bool isRunning
        {
            get
            {
                lock(mutex)
                {
                    if (timePassed) return false;
                    if (DateTime.Now - startTime < interval)
                    {
                        return true;
                    }
                    else
                    {
                        timePassed = true;
                        return false;
                    }
                }
            }
        }
    }
}