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

            //StopWatch Timer
            if (WatchIDs.Count > 0)
            {
                for (int i = 0; i < WatchIDs.Count; i++) StopWatchCountUp(WatchIDs[i]);
            }
        }

        #region StopWatch
        private List<string> WatchIDs = new List<string>();
        Dictionary<string, float> _StopWatch = new Dictionary<string, float>();
        Dictionary<string, bool> _PauseWatch = new Dictionary<string, bool>();

        public void CreateStopWatch(string myID)
        {
            if (_StopWatch.ContainsKey(myID)) return;
            WatchIDs.Add(myID);
            _StopWatch.Add(myID, 0f);
            _PauseWatch.Add(myID, false);
        }

        public void PauseStopWatch(string myID, bool myPause = true)
        {
            if (!_StopWatch.ContainsKey(myID)) return;
            _PauseWatch[myID] = myPause;
        }

        public void DeleteStopWatch(string myID)
        {
            if (!_StopWatch.ContainsKey(myID)) return;
            WatchIDs.Remove(myID);
            _StopWatch.Remove(myID);
            _PauseWatch.Remove(myID);
        }

        public void ResetStopWatch(string myID)
        {
            if (!_StopWatch.ContainsKey(myID)) return;
            _StopWatch[myID] = 0f;
        }

        public float getStopWatchTime(string myID)
        {
            return _StopWatch.ContainsKey(myID) ? _StopWatch[myID] : 0f;
        }

        void StopWatchCountUp(string myID)
        {
            if (!_StopWatch.ContainsKey(myID)) return;
            if (_PauseWatch[myID]) return;
            _StopWatch[myID] += Time.deltaTime % 60;
        }
        #endregion

        #region Individual Timer

        private List<Action> timerCallback = new List<Action>();
        private Dictionary<Action, float> timerTime = new Dictionary<Action, float>();
        private Dictionary<Action, bool> timerPause = new Dictionary<Action, bool>();

        void Countdown(Action index)
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