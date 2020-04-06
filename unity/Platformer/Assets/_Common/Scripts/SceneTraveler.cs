///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 11/02/2020 11:40
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Common {
	public class SceneTraveler : MonoBehaviour {
		private static SceneTraveler instance;
		public static SceneTraveler Instance { get { return instance; } }
		
		private void Awake(){
			if (instance){
				Destroy(gameObject);
				return;
			}
			
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		
		private void OnDestroy(){
			if (this == instance) instance = null;
		}
	}
}