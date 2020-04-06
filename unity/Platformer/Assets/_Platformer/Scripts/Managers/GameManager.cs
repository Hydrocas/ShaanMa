///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 21/01/2020 11:36
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.Data;
using Com.IsartDigital.Common.Objects;
using System;
using UnityEngine;

namespace Com.Isartdigital.Platformer.Managers {

    public delegate void GameManagerEventHandler(GameManager sender);

    public class GameManager : GameplayObject {

        [SerializeField] private LevelManager levelManager;

        public event Action<float> OnTimeChangedPercent;

        public event GameManagerEventHandler OnWin;
        public event GameManagerEventHandler OnLoose;



        public void LoadLevel(int level, Action callback) 
        {
            levelManager.OnLevelFinish += LevelManager_OnLevelFinish;
            levelManager.OnTimeLoose += LevelManager_OnTimeLoose;

            levelManager.OnTimeChangedPercent += LevelManager_OnTimeChangedPercent;

            levelManager.LoadLevel(level, callback);
        }

        private void LevelManager_OnTimeChangedPercent(TimeManager timeManager, float timePercent) {
            OnTimeChangedPercent?.Invoke(timePercent);
        }

        public void Quit(Action callback)
        {
            RemoveEvents();
            levelManager.RemoveLevel(callback);
        }

        public void Restart(Action callback)
        {
            levelManager.OnTimeLoose += LevelManager_OnTimeLoose;
            levelManager.Restart(callback);
        }

        private void LevelManager_OnTimeLoose(LevelManager sender)
        {
            levelManager.OnTimeLoose -= LevelManager_OnTimeLoose;
            Loose();
        }

        public void Loose()
        {
            OnLoose?.Invoke(this);
        }

        private void LevelManager_OnLevelFinish(LevelManager sender) 
        {
            Win();
        }

        public void Win()
        {
            levelManager.UpdateLevelSave(LevelManager_UpdateLevelSave);
        }

        private void LevelManager_UpdateLevelSave()
        {
            OnWin?.Invoke(this);
        }

        public override void RemoveEvents()
        {
            levelManager.OnLevelFinish -= LevelManager_OnLevelFinish;
            levelManager.OnTimeLoose -= LevelManager_OnTimeLoose;
        }

        internal void GetUsers(Action<User[]> callback)
        {
            levelManager.GetUsers(callback);
        }
    }
}