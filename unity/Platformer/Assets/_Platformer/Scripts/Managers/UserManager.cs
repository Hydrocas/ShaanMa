///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 24/01/2020 18:08
///-----------------------------------------------------------------

using UnityEngine;
using System;
using Com.Isartdigital.Platformer.Data;
using Com.IsartDigital.Common.Objects;

namespace Com.Isartdigital.Platformer.Managers {

    [RequireComponent(typeof(WebRequestManager), typeof(FileManager))]
    public class UserManager : GameplayObject {

        [SerializeField] private bool isLocal;
        [SerializeField] private bool isOffline;

        private WebRequestManager webRequestManager;
        private FileManager fileManager;

        public User User
        {
            get
            {
                if (isOffline) return fileManager.User;
                return webRequestManager.User;
            }
        }

        private void Awake() 
        {
            webRequestManager = GetComponent<WebRequestManager>();
            fileManager = GetComponent<FileManager>();

            webRequestManager.Init(isLocal, fileManager);
            fileManager.Init();
        }

        public void Login(string username, Action callback) 
        {
            if (isOffline) fileManager.Login(username, callback);
            else
            {
                webRequestManager.OnLoginFail += WebRequestManager_OnLoginFail;
                webRequestManager.OnLoginSuccess += WebRequestManager_OnLoginSuccess;

                webRequestManager.Login(username, callback);
            }
        }

        private void WebRequestManager_OnLoginFail(string username, Action callback)
        {
            isOffline = true;
            RemoveEvents();
            fileManager.Login(username, callback);
        }

        private void WebRequestManager_OnLoginSuccess(Action callback)
        {
            RemoveEvents();
            callback();
        }

        public void GetUsersLevelSave(int levelIndex, Action<User[]> callback) 
        {
            if (isOffline) fileManager.GetUsersLevelSaves(levelIndex, callback);
            else webRequestManager.GetUsersLevelSaves(levelIndex, callback);
        }

        public void UpdateLevelSave(LevelSave levelSave, int levelIndex, Action callback) 
        {
            if (isOffline) fileManager.UpdateLevelSave(levelSave, levelIndex, callback);
            else webRequestManager.UpdateLevelSave(levelSave, levelIndex, callback);
        }

        public override void RemoveEvents()
        {
            webRequestManager.OnLoginFail -= WebRequestManager_OnLoginFail;
            webRequestManager.OnLoginSuccess -= WebRequestManager_OnLoginSuccess;
        }
    }
}