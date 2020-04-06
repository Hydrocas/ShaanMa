///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 23/03/2020 16:31
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.UI {
	public class UiBackground : MonoBehaviour {
		private static UiBackground instance;
		public static UiBackground Instance { get { return instance; } }
		
		private void Awake(){
			if (instance){
				Destroy(gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		
		private void Start () {
			
		}
		
		private void Update () {
			
		}
		
		private void OnDestroy(){
			if (this == instance) instance = null;
		}
	}
}