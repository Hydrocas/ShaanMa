///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER && Eliaz LEBERT
/// Date : 04/02/2020 16:24
///-----------------------------------------------------------------

using UnityEngine;
using Com.IsartDigital.Common.Objects;

namespace Com.Isartdigital.Platformer.LevelObjects.Obstacles.Turret {
	public class Shoot : GameplayObject {

		[SerializeField] private GameObject BurnMark;
		[SerializeField] private bool killsPlayer;
		private float speed;
		private float friction;
		private float lifeTime;
		private float shootGrowth;

		public void Init(float speed, float shootLifeTime, float friction, float ShootSize, float ShootGrowth, bool mute)
		{
			this.speed = speed;
			this.friction = friction;
			Destroy(this.gameObject, shootLifeTime);
			lifeTime = shootLifeTime;
			this.GetComponent<Rigidbody2D>().AddForce(transform.right * speed / 300);
			transform.localScale *= ShootSize;
			this.shootGrowth = 1 + ShootGrowth/100;
			if (mute) GetComponent<AudioSource>().mute = true;
		}
		
		private void FixedUpdate () {
			this.GetComponent<Rigidbody2D>().velocity *= friction;
			transform.localScale *= shootGrowth;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.CompareTag("FireGoesThrough"))return ;
			GetComponent<AudioSource>().Play();

			if (collision.gameObject.GetComponent<PlayerScript.Physics.PlayerPhysique>() && killsPlayer) collision.gameObject.GetComponent<PlayerScript.Physics.PlayerPhysique>().Die();
			else
			{
				GameObject burn = Instantiate(BurnMark);
				burn.transform.parent = collision.gameObject.transform;
				burn.transform.position = transform.position;
				burn.transform.localScale *= 2;
				burn.transform.rotation = transform.rotation;
				burn.transform.Rotate(new Vector3(0, 0, 90));
				Destroy(burn, 5);
				
				this.GetComponent<Rigidbody2D>().velocity *= -0.1f;
				this.GetComponent<CapsuleCollider2D>().enabled = false;
				Destroy(this.gameObject, 0.5f);
			}
		}
	}
}