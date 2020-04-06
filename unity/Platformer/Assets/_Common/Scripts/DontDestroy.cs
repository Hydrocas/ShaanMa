///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 23/03/2020 16:08
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Common {
	public class DontDestroy : MonoBehaviour {
	
		private void Start () {
			DontDestroyOnLoad(gameObject);
		}
	
	}
}