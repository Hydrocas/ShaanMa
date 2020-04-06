///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 21/01/2020 11:36
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.Isartdigital.Platformer.Managers {

    public class TimeManager {

        private float timeSpan;
        private float timeLimit;

        public event Action<TimeManager,float> OnTimeChangePercent;

        public float TimeSpan => timeSpan;
        public float TimeLimit => timeLimit;

        public bool CalcTime() {
            timeSpan += Time.deltaTime;

            if (TimeSpan >= TimeLimit) {
                timeSpan = TimeLimit;
            }

            OnTimeChangePercent?.Invoke(this,TimeSpan / timeLimit);
            return timeSpan >= timeLimit;
        }

        public TimeManager(float timeLimit, CollectibleManager collectibleManager) {
            this.timeLimit = timeLimit;

            collectibleManager.OnCollectibleCollected += CollectibleManager_OnCollectibleCollected;
        }

        private void CollectibleManager_OnCollectibleCollected(CollectibleManager collectibleManager, float timeEarned) {
            timeSpan -= timeEarned;

            if (timeSpan < 0) {
                timeSpan = 0;
            }
        }
    }
}