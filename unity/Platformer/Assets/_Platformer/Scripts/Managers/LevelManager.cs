///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 21/01/2020 11:36
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.Data;
using Com.Isartdigital.Platformer.LevelObjects;
using Com.Isartdigital.Platformer.PlayerScript;
using Com.Isartdigital.Platformer.PlayerScript.Physics;
using Com.IsartDigital.Common.Objects;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.Isartdigital.Platformer.Managers {

    public class LevelManager : GameplayObject {

        public event Action<LevelManager> OnLevelFinish;
        public event Action<LevelManager> OnTimeLoose;
        public event Action<TimeManager, float> OnTimeChangedPercent;

        [SerializeField] private string mainSceneName = "UI";
        [SerializeField] private string levelSceneName = "Level{0}";
        [SerializeField] private string assetSceneName = "GA";
        [SerializeField] private UserManager userManager;

        // managers
        private CheckpointManager checkpointManager;
        private CollectibleManager collectibleManager;
        private DoorManager doorManager;
        private TimeManager timeManager;

        private int currentIndex;
        private Level currentLevel;
        private PlayerPhysique player;

        private bool isLost = false;

        private int deathCount;

        public float TimeRatio => timeManager.TimeSpan / timeManager.TimeLimit;

        public int DeathCount  => deathCount;
        public float TimeSpan => timeManager.TimeSpan;

        public void LoadLevel(int index, Action callback = null) {
            currentIndex = index;
            StartCoroutine(LoadLevelEnum(string.Format(levelSceneName, index), callback));
        }

        private IEnumerator LoadLevelEnum(string name, Action callback = null) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

            while (!asyncLoad.isDone) {
                yield return null;
            }

            InitLevel();
            callback?.Invoke();
        }

        public void LoadAssetTest(Action callback = null) {
            StartCoroutine(LoadSceneEnum(assetSceneName, callback));
        }

        public void RemoveLevel(Action callback = null) {
            StartCoroutine(LoadSceneEnum(mainSceneName, callback));
        }

        private IEnumerator LoadSceneEnum(string name, Action callback = null) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

            while (!asyncLoad.isDone) 
            {
                yield return null;
            }

            callback?.Invoke();
        }

        public void InitLevel() {
            isLost = false;

            currentLevel = FindObjectOfType<Level>();
            player = FindObjectOfType<PlayerPhysique>();

            checkpointManager = new CheckpointManager(currentLevel.CheckpointList);
            collectibleManager = new CollectibleManager(currentLevel.CollectibleList);
            doorManager = new DoorManager(currentLevel.DoorList);
            timeManager = new TimeManager(currentLevel.TimeLimit, collectibleManager);

            timeManager.OnTimeChangePercent += TimeManager_OnTimeChangePercent;

            checkpointManager.OnCheckpointPassed += doorManager.DoorManager_OnCheckpointPassed;

            player.OnDie += Player_OnDie;
            currentLevel.Goal.OnTrigger += Goal_OnTrigger;

            deathCount = 0;
        }

        private void TimeManager_OnTimeChangePercent(TimeManager timeManager,float timePercent) {
            OnTimeChangedPercent?.Invoke(timeManager, timePercent);
        }

        private void Player_OnDie(PlayerPhysique player) {
            deathCount++;
            StartCoroutine(WaitForRespawn());
        }

        private IEnumerator WaitForRespawn() {

            yield return new WaitForSeconds(1);

            RespawnPlayer();
        }

        private void Goal_OnTrigger(Goal goal) {
            RemoveEvents();
            OnLevelFinish?.Invoke(this);
        }

        public void UpdateLevelSave(Action callback) 
        {
            LevelSave levelSave = new LevelSave(timeManager.TimeSpan, collectibleManager.Count, DeathCount);

            userManager.UpdateLevelSave(levelSave, currentIndex - 1, callback);

            timeManager = null;
        }

        public void GetUsers(Action<User[]> callback) {
            userManager.GetUsersLevelSave(currentIndex - 1, callback);
        }

        public void Restart(Action callback) {
            LoadLevel(currentIndex, callback);
        }

        private void RespawnPlayer() {
            player.Respawn(checkpointManager.LastCheckpointPos);
            doorManager.ResetDoors();
        }

        private void Update()
        {
            if (timeManager == null) return;

            bool isTimeLost = timeManager.CalcTime();

            if (isTimeLost && !isLost)
            {
                isLost = true;
                RemoveEvents();
                OnTimeLoose?.Invoke(this);
            } 
        }

        public override void RemoveEvents()
        {
            player.OnDie -= Player_OnDie;
            currentLevel.Goal.OnTrigger -= Goal_OnTrigger;

        }
    }
}