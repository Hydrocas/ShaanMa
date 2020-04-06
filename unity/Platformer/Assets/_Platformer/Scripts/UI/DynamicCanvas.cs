///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 23/03/2020 16:43
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.Isartdigital.Platformer.UI {
	public class DynamicCanvas : MonoBehaviour
	{
		private Canvas canvas;

		private void Start()
		{
			canvas = GetComponent<Canvas>();

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
				SceneManager.sceneUnloaded -= SceneManager_SceneUnloaded;
				SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
			
		}
	}

}
		