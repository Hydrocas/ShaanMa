///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 24/01/2020 14:44
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.PlayerScript.Physics;
using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects {
    [RequireComponent(typeof(Collider2D))]
	public class Killzone : MonoBehaviour {

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<PlayerPhysique>().Die();
            }
        }
    }
}