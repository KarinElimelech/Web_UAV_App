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

        /**
         * CTOR
         */
        private TimeTracker()
        {
            timePassed = false;
            startTime = DateTime.MinValue;
        }

        /**
         * singleton
         */
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

        /**
         * set the interval of time tracker
         */
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

        /**
         * init timer
         */
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

        /**
         * bool funct- true if the time tracker still running, false - else.
         * check the time from the begin to curr
         */
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