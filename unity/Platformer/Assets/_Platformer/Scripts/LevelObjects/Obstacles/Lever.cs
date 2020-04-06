///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 04/02/2020 13:02
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects.Obstacles {

	public delegate void LeverEventHandler(Lever sender);
	[RequireComponent(typeof(Animator), typeof(Collider2D))]
	public class Lever : MonoBehaviour {

		[SerializeField] private string triggerTag = "Player";
		[SerializeField] private string animationName = "Active";
		[SerializeField] private Transform Engrenage;
		[SerializeField] private float spinSpeed = 600;
		private float currentSpinSpeed = 0;
		private float friction = 0.99f;

		public event LeverEventHandler OnTriggerTag;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!collision.CompareTag(triggerTag)) return;

			OnTriggerTag?.Invoke(this);

			GetComponent<Collider2D>().enabled = false;
			GetComponent<Animator>().SetTrigger(animationName);
			currentSpinSpeed = spinSpeed;
		}

        public void ResetPosition() {
            GetComponent<Collider2D>().enabled = true;
        }

		private void Update()
		{
			if (currentSpinSpeed == 0) return;

			Engrenage.Rotate(Vector3.forward * -currentSpinSpeed * Time.deltaTime);
			currentSpinSpeed *= friction;
			currentSpinSpeed -= friction;
			if (currentSpinSpeed < 0) currentSpinSpeed = 0;
		}

	}
}