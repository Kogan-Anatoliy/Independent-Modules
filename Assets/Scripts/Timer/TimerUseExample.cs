/*
 * autor: Kogan Anatolii
 * e-mail: a.kogan@gooligames.com
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using Gooligames.Timers;

namespace Gooligames.Tests
{
    public class TimerUseExample : MonoBehaviour
    {
        public string timerKey = "test";

        public int minutes = 1;
        public int seconds = 50;

        public Text text = null;

        private TimerController _timer = null;

        private void Awake()
        {
            Initilize();
        }

        [ContextMenu("Delete Save")]
        public void DeleteSave()
        {
            TimerSaver.DeleteSave(timerKey);
        }

        [ContextMenu("Restart Timer")]
        public void RestartTimer()
        {
            TimerController.GetTimerByKey(timerKey).Start(DateTime.Now.AddMinutes(minutes).AddSeconds(seconds));
        }

        private void Initilize()
        {
            _timer = TimerController.GetTimerByKey(timerKey);

            _timer.OnValueChangedEvent += DisplayTimerValue;
            _timer.OnFinishedEvent += OnTimerFinished;

            if (TimerSaver.TimerExists(timerKey) == false)
            {
                _timer.Start(DateTime.Now.AddMinutes(minutes).AddSeconds(seconds));
            }
            else
            {
                _timer.Start();
            }

            DisplayTimerValue();
        }

        private void DisplayTimerValue()
        {
            text.text = $"{_timer.values.days}:{_timer.values.hours}:{_timer.values.minutes}:{_timer.values.seconds}";
        }

        private void OnTimerFinished()
        {
            Debug.Log("Timer Finished");
        }

        private void OnDestroy()
        {
            _timer.Stop();
        }
    }
}