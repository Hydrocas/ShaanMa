///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/02/2020 11:02
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.Data;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Com.Isartdigital.Platformer.Managers {
	public class WebRequestManager : MonoBehaviour {

        [Header("Path")]
        [SerializeField] private string localPath = "http://localhost:8000";
        [SerializeField] private string cloudPath = "https://platformererable.herokuapp.com";
        [SerializeField] private string loginPath = "/login";
        [SerializeField] private string mePath = "/me";
        [SerializeField] private string updateLevelSavePath = "/updateLevelSave/{0}";
        [SerializeField] private string usersPath = "/users";
        [Space]

        [Header("Error Messages")]
        [SerializeField] private string networkErrorTxt = "NetworkError: {0}";
        [SerializeField] private string httpErrorTxt = "{0}: {1}";


        private const string AUTHORIZATION = "Authorization";
        private const string BEARER = "Bearer ";
        private const string CONTENT_TYPE = "application/json";
        private const string HEADER_PARAM = "/{0}";

        private User user;
        private string token;
        private string mainPath;
        private bool isLocal;
        private FileManager fileManager;

        public User User => user;

        public event Action<string, Action> OnLoginFail;
        public event Action<Action> OnLoginSuccess;
        public event Action<WebRequestManager> OnConnectionLost;

        public void Init(bool isLocal, FileManager fileManager)
        {
            this.isLocal = isLocal;
            this.fileManager = fileManager;

            mainPath = isLocal ? localPath : cloudPath;
        }

        public void Login(string username, Action callback)
        {
            StartCoroutine(LoginRoutine(username, callback));
        }

        public void GetUsersLevelSaves(int levelIndex, Action<User[]> callback)
        {
            StartCoroutine(UsersLevelSaveRoutine(levelIndex, callback));
        }

        public void UpdateLevelSave(LevelSave levelSave, int levelIndex, Action callback)
        {
            StartCoroutine(UpdateLevelSaveRoutine(levelSave, levelIndex, callback));
        }

        private IEnumerator LoginRoutine(string username, Action callback)
        {
            string url = mainPath + usersPath + loginPath;
            string json = JsonUtility.ToJson(new User(username));

            using (UnityWebRequest req = GetWebRequest(url, UnityWebRequest.kHttpVerbPOST, json))
            {
                yield return req.SendWebRequest();

                if (req.isNetworkError)
                {
                    LogNetworkError(req);

                    OnLoginFail?.Invoke(username, callback);
                }
                else if (req.isHttpError)
                {
                    LogHttpError(req);

                    if (!isLocal) OnLoginFail?.Invoke(username, callback);
                }
                else
                {
                    token = req.downloadHandler.text;

                    StartCoroutine(WriteFileUserRoutine());
                    StartCoroutine(MeRoutine(callback));
                }
            }
        }

        private IEnumerator MeRoutine(Action callback)
        {
            string url = mainPath + usersPath + mePath;

            using (UnityWebRequest req = GetWebRequest(url, UnityWebRequest.kHttpVerbGET))
            {
                yield return req.SendWebRequest();

                if (req.isNetworkError)
                {
                    LogNetworkError(req);
                    OnConnectionLost?.Invoke(this);
                }
                else if (req.isHttpError) LogHttpError(req);
                else
                {
                    user = JsonUtility.FromJson<User>(req.downloadHandler.text);
                    OnLoginSuccess?.Invoke(callback);
                }
            }
        }

        private IEnumerator UsersLevelSaveRoutine(int levelIndex, Action<User[]> callback)
        {
            string url = mainPath + usersPath + string.Format(HEADER_PARAM, levelIndex);

            using (UnityWebRequest req = GetWebRequest(url, UnityWebRequest.kHttpVerbGET))
            {
                yield return req.SendWebRequest();

                if (req.isNetworkError)
                {
                    LogNetworkError(req);
                    OnConnectionLost?.Invoke(this);
                }
                else if (req.isHttpError) LogHttpError(req);
                else
                {
                    User[] users = JsonUtility.FromJson<Users>(req.downloadHandler.text).list.ToArray();
                    callback(users);
                }
            }
        }

        private IEnumerator UpdateLevelSaveRoutine(LevelSave levelSave, int levelIndex, Action callback)
        {
            levelSave = levelSave.Combine(user.levelSaves[levelIndex]);

            string json = JsonUtility.ToJson(levelSave);
            string url = mainPath + usersPath + string.Format(updateLevelSavePath, levelIndex);

            using (UnityWebRequest req = GetWebRequest(url, UnityWebRequest.kHttpVerbPOST, json))
            {
                yield return req.SendWebRequest();

                if (req.isNetworkError)
                {
                    LogNetworkError(req);
                    OnConnectionLost?.Invoke(this);
                }
                else if (req.isHttpError) LogHttpError(req);
                else
                {
                    user.levelSaves[levelIndex] = levelSave;
                    callback();
                }
            }
        }

        private IEnumerator WriteFileUserRoutine()
        {
            string url = mainPath + usersPath;

            using (UnityWebRequest req = GetWebRequest(url, UnityWebRequest.kHttpVerbGET))
            {
                yield return req.SendWebRequest();

                if (req.isNetworkError)
                {
                    LogNetworkError(req);
                    OnConnectionLost?.Invoke(this);
                }
                else if (req.isHttpError) LogHttpError(req);
                else
                {
                    fileManager.WriteFile(req.downloadHandler.text);
                }
            }
        }

        private UnityWebRequest GetWebRequest(string url, string verb)
        {
            UnityWebRequest req = new UnityWebRequest(url, verb);

            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader(AUTHORIZATION, BEARER + token);

            return req;
        }

        private UnityWebRequest GetWebRequest(string url, string verb, string json)
        {
            UnityWebRequest req = GetWebRequest(url, verb);

            req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            req.uploadHandler.contentType = CONTENT_TYPE;

            return req;
        }

        private void LogNetworkError(UnityWebRequest req)
        {
            Debug.LogError(string.Format(networkErrorTxt, req.error));
        }

        private void LogHttpError(UnityWebRequest req)
        {
            Debug.LogError(string.Format(httpErrorTxt, req.error, req.downloadHandler.text));
        }
    }
}