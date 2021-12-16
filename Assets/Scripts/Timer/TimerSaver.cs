/*
 * autor: Kogan Anatolii
 * e-mail: a.kogan@gooligames.com
 */
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Gooligames.Timers
{
    public static class TimerSaver
    {
        public const string DirectoryName = "TimersData";
        public const string FileTimerValuesEnding = "Value.timer";
        public const string FileStopDateEnding = "StopDate.timer";

        public static void SaveTimer(this TimerController timer)
        {
            var directory = Directory.CreateDirectory($"{Application.persistentDataPath}/{DirectoryName}");
            var pathWithValues = $"{directory.FullName}/{timer.key}{FileTimerValuesEnding}";
            var pathWithStopDate = $"{directory.FullName}/{timer.key}{FileStopDateEnding}";

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(pathWithValues, FileMode.Create))
            {
                formatter.Serialize(stream, timer.values);
            }

            using (FileStream stream = new FileStream(pathWithStopDate, FileMode.Create))
            {
                formatter.Serialize(stream, new TimerStopDate(DateTime.Now));
            }
        }

        public static void SetValues(this TimerController timer)
        {
            var directory = Directory.CreateDirectory($"{Application.persistentDataPath}/{DirectoryName}");
            var pathWithValues = $"{directory.FullName}/{timer.key}{FileTimerValuesEnding}";
            var pathWithStopDate = $"{directory.FullName}/{timer.key}{FileStopDateEnding}";

            if (File.Exists(pathWithValues))
            {
                TimerData timerValues;
                TimerStopDate timerStopDate;

                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(pathWithValues, FileMode.Open))
                {
                    timerValues = (TimerData)formatter.Deserialize(stream);
                }

                using (FileStream stream = new FileStream(pathWithStopDate, FileMode.Open))
                {
                    timerStopDate = (TimerStopDate)formatter.Deserialize(stream);
                }

                var interval = DateTime.Now.SetStopData(timerStopDate).AddDays(timerValues.days).AddHours(timerValues.hours).AddMinutes(timerValues.minutes).AddSeconds(timerValues.seconds) 
                    - DateTime.Now;

                timer.values = new TimerData(interval);
            }
        }

        public static bool DeleteSave(string key)
        {
            var directory = Directory.CreateDirectory($"{Application.persistentDataPath}/{DirectoryName}");
            var pathWithValues = $"{directory.FullName}/{key}{FileTimerValuesEnding}";
            var pathWithStopDate = $"{directory.FullName}/{key}{FileStopDateEnding}";

            if (File.Exists(pathWithValues))
            {
                File.Delete(pathWithValues);
                File.Delete(pathWithStopDate);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <returns>true - if timer save by this key exists</returns>
        public static bool TimerExists(string key)
        {
            if (Directory.Exists($"{Application.persistentDataPath}/{DirectoryName}") == true)
            {
                return File.Exists($"{Application.persistentDataPath}/{DirectoryName}/{key}{FileTimerValuesEnding}");
            }
            else
            {
                return false;
            }
        }

        [Serializable]
        private struct TimerStopDate
        {
            public int seconds;
            public int minutes;
            public int hours;
            public int days;

            public TimerStopDate(DateTime stopTime)
            {
                days = stopTime.Day;
                hours = stopTime.Hour;
                minutes = stopTime.Minute;
                seconds = stopTime.Second;
            }
        }

        private static DateTime SetStopData(this DateTime data, TimerStopDate stopDate)
        {
            data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, stopDate.days, stopDate.hours, stopDate.minutes, stopDate.seconds);
            return data;
        }
    }
}