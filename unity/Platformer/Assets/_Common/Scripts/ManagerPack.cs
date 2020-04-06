///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 11/02/2020 14:15
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Common {
	public class ManagerPack : MonoBehaviour {
		private static ManagerPack instance;
		public static ManagerPack Instance { get { return instance; } }
		
		private void Awake(){
			if (instance){
				Destroy(gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		
		private void OnDestroy(){
			if (this == instance) instance = null;
		}
	}
}