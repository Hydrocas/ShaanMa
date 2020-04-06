///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/02/2020 11:03
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.Data;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Com.Isartdigital.Platformer.Managers {
	public class FileManager : MonoBehaviour {

        [SerializeField] private string fileName = "users.json";

        private User user;
        private Users users;
        private string filePath;

        public User User => user;

        public void Init()
        {
            filePath = Application.persistentDataPath + fileName;
        }

        public void Login(User user)
        {
            this.user = user;

            if (!File.Exists(filePath))
            {
                users = new Users();
                users.list = new List<User>() { user };

                File.WriteAllText(filePath, JsonUtility.ToJson(users));
            }

            users = JsonUtility.FromJson<Users>(File.ReadAllText(filePath));
        }

        public void WriteFile(string json)
        {
            File.WriteAllText(filePath, json);
        }

        public void Login(string username, Action callback)
        {
            List<User> userList;

            if (!File.Exists(filePath))
            {
                user = new User(username);

                userList = new List<User>();
                userList.Add(User);

                users = new Users();
                users.list = userList;

                File.WriteAllText(filePath, JsonUtility.ToJson(users));

                callback();
                return;
            }

            users = JsonUtility.FromJson<Users>(File.ReadAllText(filePath));
            userList = users.list;

            string usernameUpper = username.ToUpper();

            for (int i = userList.Count - 1; i >= 0; i--)
            {
                if (userList[i].username.ToUpper() != usernameUpper) continue;

                user = userList[i];
                callback();

                return;
            }

            user = new User(username);
            userList.Add(User);

            File.WriteAllText(filePath, JsonUtility.ToJson(users));

            callback();
        }

        public void GetUsersLevelSaves(int levelIndex, Action<User[]> callback)
        {
            List<User> userList = new List<User>(users.list);

            User user;
            LevelSave levelSave;

            for (int i = 0; i < userList.Count; i++)
            {
                user = userList[i];

                if (levelIndex >= user.levelSaves.Length) continue;

                levelSave = user.levelSaves[levelIndex];

                userList[i].levelSaves = new LevelSave[] { levelSave };
            }

            userList.Sort(CompareUserByTime);

            callback(userList.ToArray());
        }

        public static int CompareUserByTime(User userA, User userB)
        {
            float timeSpanA = userA.levelSaves[0].timeSpan;
            float timeSpanB = userB.levelSaves[0].timeSpan;

            if (timeSpanA < timeSpanB) return 1;

            if (timeSpanA > timeSpanB) return -1;

            return 0;
        }

        public void UpdateLevelSave(LevelSave levelSave, int levelIndex, Action callback)
        {
            if (levelIndex >= user.levelSaves.Length)
            {
                user.levelSaves[levelIndex] = levelSave;
                callback();
                return;
            }

            LevelSave currentLevelSave = user.levelSaves[levelIndex];
            user.levelSaves[levelIndex] = (currentLevelSave == null) ? levelSave : levelSave.Combine(currentLevelSave);

            File.WriteAllText(filePath, JsonUtility.ToJson(users));

            callback();
        }
    }
}