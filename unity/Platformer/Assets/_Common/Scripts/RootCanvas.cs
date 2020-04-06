///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 11/02/2020 14:16
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.Isartdigital.Common {
	[RequireComponent(typeof(Canvas))]
	public class RootCanvas : MonoBehaviour 
	{
		private static RootCanvas instance;
		public static RootCanvas Instance => instance;

		private Canvas canvas;
		
		private void Awake()
		{
			canvas = GetComponent<Canvas>();

			if (!canvas.isRootCanvas)
			{
				Debug.LogError("RootCanvas is not higher parent");
			}

			if (instance)
			{
				Destroy(gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);
			instance = this;

			if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				SceneManager.sceneUnloaded += SceneManager_SceneUnloaded;
				SceneManager.sceneLoaded += SceneManager_SceneLoaded;
			}
		}

		private void SceneManager_SceneUnloaded(Scene arg0)
		{
			canvas.worldCamera = null;
		}

		private void SceneManager_SceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			canvas.worldCamera = Camera.main;
		}

		private void OnDestroy()
		{
			if (this == instance)
			{
				SceneManager.sceneUnloaded -= SceneManager_SceneUnloaded;
				SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
				instance = null;
			}
		}
	}
}