using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * Author: Simon Grunwald
 * Created: 09.01.2023
 * Edited: 08.07.2023
 */

namespace Base
{
    public class GlobalTimer : MonoBehaviour
    {
        public static GlobalTimer Timer;


        private void Awake()
        {
            if (Timer != null && Timer != this) Destroy(this);
            else Timer = this;
        }

        void Update()
        {
            //Induvidial Timer
            if(timerCallback.Count > 0)
            {
                for (int i = 0; i < timerCallback.Count; i++) Countdown(timerCallback[i]);
            }

            //Async Timer
            if (timer.Count > 0)
            {
                for (int i = 0; i < timer.Count; i++) AsyncCountdown(i);
            }
        }

        

        #region Individual Timer

        private List<Action> timerCallback = new List<Action>();
        private Dictionary<Action, float> timerTime = new Dictionary<Action, float>();
        private Dictionary<Action, bool> timerPause = new Dictionary<Action, bool>();

        public void Countdown(Action index)
        {
            if (timerPause[index]) return;
            if (timerTime[index] > 0)
            {
                timerTime[index] -= Time.deltaTime % 60;
                if (isTimerComplete(index))
                {
                    RemoveTimer(index);
                    index();
                }
            }
        }

        void RemoveTimer(Action index)
        {
            timerTime.Remove(index);
            timerPause.Remove(index);
            timerCallback.Remove(index);
        }

        public void SetTimer(float myTime, Action myTimerCallback)
        {
            if (timerTime.ContainsKey(myTimerCallback)) return;
            timerCallback.Add(myTimerCallback);
            timerTime.Add(myTimerCallback, myTime);
            timerPause.Add(myTimerCallback, false);
        }
        
        public float GetTimer(Action myTimerCallback)
        {
            if (!timerTime.ContainsKey(myTimerCallback)) return 0f;
            return timerTime[myTimerCallback];
        }

        public void PauseTimer(Action myTimerCallback, bool myisPausingTimer = true)
        {
            if (!timerTime.ContainsKey(myTimerCallback)) return;
            timerPause[myTimerCallback] = myisPausingTimer;
        }

        public void ModifyTimer(float myModifier, Action myTimerCallback, bool myisPausingTimer = true)
        {
            if (!timerTime.ContainsKey(myTimerCallback)) return;
            timerPause[myTimerCallback] = myisPausingTimer;
            timerTime[myTimerCallback] += myModifier;
        }

        public void DeleteTimer(Action myTimerCallback)
        {
            if (!timerTime.ContainsKey(myTimerCallback)) return;
            RemoveTimer(myTimerCallback);
        }
        
        public float GetTimerStatus(Action myAction)
        {
            if (timerTime.ContainsKey(myAction)) return timerTime[myAction];
            else return 0f;
        }

        bool isTimerComplete(Action index)
        {
            return timerTime[index] <= 0f;
        }
        #endregion

        /// <summary>
        /// The Async Timer handles actions that should be allowed to stack, the downside is that this timer is not able to be modified while it runs
        /// </summary>
        #region Async Timer

        private List<float> timer = new List<float>();
        private List<Action> AsyncCallback = new List<Action>();

        public void SetTimerAsync(float myTime, Action myTimerCallback)
        {
                timer.Add(myTime);
                AsyncCallback.Add(myTimerCallback);
        }

        public void AsyncCountdown(int index)
        {
            if (timer[index] > 0)
            {
                timer[index] -= Time.deltaTime % 60;
                if (isAsyncTimerComplete(index))
                {
                    AsyncCallback[index]();
                    AsyncRemoveTimer(index);
                }
            }
        }

        void AsyncRemoveTimer(int index)
        {
            //time.RemoveAt(index);
            timer.RemoveAt(index);
            AsyncCallback.RemoveAt(index);
        }

        bool isAsyncTimerComplete(int index)
        {
            return timer[index] <= 0f;
        }

        #endregion
    }
}