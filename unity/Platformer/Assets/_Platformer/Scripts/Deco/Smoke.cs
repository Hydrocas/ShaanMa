///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 20/03/2020 20:49
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.Deco {
	public class Smoke : MonoBehaviour {

        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;

        private Animator _animator;

		private void Start () {
            _animator = GetComponent<Animator>();
            _animator.speed = Random.Range(minSpeed, maxSpeed);
		}
		
		private void Update () {
			
		}
	}
}