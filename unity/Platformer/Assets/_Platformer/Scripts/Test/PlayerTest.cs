///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 20/01/2020 11:20
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.Test {
	public class PlayerTest : MonoBehaviour {
		[SerializeField] private float speed;
		private void Update () {
			Vector3 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			transform.position += direction * (speed * Time.deltaTime);
		}
	}
}