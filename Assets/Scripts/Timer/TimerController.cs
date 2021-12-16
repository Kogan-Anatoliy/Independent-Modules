/*
 * autor: Kogan Anatolii
 * e-mail: a.kogan@gooligames.com
 */
using System;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;
using UnityEngine;
using Gooligames.Tests;

namespace Gooligames.Timers
{
    public sealed class TimerController
    {
        public static List<TimerController> runningTimers = new List<TimerController>(0);

        public string key { get; private set; } = "simple";

        private bool _isRunning = false;
        public bool IsRunning
        {
            get { return _isRunning; }
            private set
            {
                _isRunning = value;
                if (_isRunning == true)
                {
                    if (TimerSaver.TimerExists(key))
                    {
                        this.SetValues();
                    }

                    //fix the possibility of double starting TimerStateAsyncUpdate
                    if (_timeTickingFoo == null)
                    {
                        _timeTickingFoo = TimerStateAsyncUpdate;
                        _timeTickingFoo();
                    }

                    runningTimers.Add(this);
                }
                else
                {
                    this.SaveTimer();
                    runningTimers.Remove(runningTimers.Find(p => p == this));
                }
            }
        }

        public TimerData values;

        public delegate void OnTimePessHandler();
        public event OnTimePessHandler OnFinishedEvent;
        public event OnTimePessHandler OnValueChangedEvent;

        private OnTimePessHandler _timeTickingFoo = null;

        private TimerController(string key)
        {
            this.key = key;
        }

        /// <returns>If timer already running, return it; one key - one timer</returns>
        public static TimerController GetTimerByKey(string key)
        {
            foreach (var timer in runningTimers)
            {
                if (timer.key == key)
                {
                    return timer;
                }
            }

            return new TimerController(key);
        }

        /// <summary>
        /// Start timer from DateTime.Now to endTime
        /// </summary>
        /// <param name="endTime"></param>
        public void Start(DateTime endTime)
        {
            //if timer with this key existed, stop timer and delete save firstly
            Stop();
            TimerSaver.DeleteSave(key);

            TimeSpan interval = endTime - DateTime.Now;
            values = new TimerData(interval);

            IsRunning = true;
        }

        /// <summary>
        /// Use this, if timer save existed
        /// </summary>
        /// <returns>If timer run successfully, returns true; if not - use StartTimer(DateTime endTime)</returns>
        public bool Start()
        {
            if (IsRunning == false)
            {
                IsRunning = true;
                return IsRunning;
            }
            else
            {
                Debug.LogError("Timer is already running!");
                return false;
            }
        }

        public void Stop()
        {
            if (IsRunning == true)
            {
                IsRunning = false;
            }
        }

        private async void TimerStateAsyncUpdate()
        {
            while (CheckForRemainingTime(ref values.seconds))
            {
                await Task.Delay(1000);
                if (IsRunning == false) return;

                values.seconds--;
                OnValueChangedEvent?.Invoke();
            }

            IsRunning = false;
            OnFinishedEvent?.Invoke();

            TimerSaver.DeleteSave(key);
        }

        /// <summary>
        /// Equates seconds to 60, if seconds == 0 and another time parameters don't equal 0
        /// </summary>
        /// <returns>seconds > 0</returns>
        private bool CheckForRemainingTime(ref int seconds)
        {
            if (seconds <= 0)
            {
                if (values.minutes > 0)
                {
                    values.minutes--;
                    seconds += 60;
                }
                else
                {
                    if (values.hours > 0)
                    {
                        values.hours--;
                        values.minutes += 59;
                        seconds += 60;
                    }
                    else
                    {
                        if (values.days > 0)
                        {
                            values.days--;
                            values.hours += 23;
                            values.minutes += 59;
                            seconds += 60;
                        }
                    }
                }
            }

            return seconds > 0;
        }
    }

    [Serializable]
    public struct TimerData
    {
        public int seconds;
        public int minutes;
        public int hours;
        public int days;

        public TimerData(TimeSpan interval)
        {
            days = interval.Days > 0 ? interval.Days : 0;
            hours = interval.Hours > 0 ? interval.Hours : 0;
            minutes = interval.Minutes > 0 ? interval.Minutes : 0;
            seconds = interval.Seconds > 0 ? interval.Seconds : 0;
        }

        public TimerData(int days, int hours, int minutes, int seconds)
        {
            this.days = days;
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
        }
    }
}