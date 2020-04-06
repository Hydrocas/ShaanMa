///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 19/02/2020 14:27
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.Managers;
using UnityEngine;

namespace Com.Isartdigital.Platformer.Test {
	public class LevelManagerLauncher : MonoBehaviour {

		[SerializeField] private LevelManager levelManager;

		private void Start()
		{
			levelManager.InitLevel();
		}
	}
}