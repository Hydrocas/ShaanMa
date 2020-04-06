///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER && Eliaz LEBERT
/// Date : 19/02/2020 14:47
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Objects;
using System.Collections;
using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects.Obstacles.TurretSpinning
{
	public class TurretSpinning : StateObject
	{
		[Header("General settings")]
		[SerializeField] private Transform canon;
		[SerializeField] private Transform shootSpawn;
		[SerializeField] private Turret.Shoot shootPrefab;
		[SerializeField] private LayerMask GroundLayer;
		[Header("Personal settings")]
		[SerializeField] private bool RotationIsClockwise;
		[SerializeField] private float RotationSpeed = 30;
		[SerializeField] private float ShotSpeed = 50;
		[SerializeField] private float ShotFriction = 0.95f;
		[SerializeField] private float FireRate = 0.01f;
		[SerializeField] private int ShootPerFire = 1;
		[SerializeField] private float FireRateNoise = 0f;
		[SerializeField] private float ShootAngleNoisePlus = 0f;
		[SerializeField] private float ShootAngleNoiseMinus = 0f;
		[SerializeField] private float ShootSpeedNoisePlus = 0f;
		[SerializeField] private float ShootSpeedNoiseMinus = 0f;
		[SerializeField] private float ShootLifeTime = 1f;
		[SerializeField] private float DetectGroundDistance = 2f;
		[SerializeField] private float ShootSize = 1f;
		[SerializeField] private float ShootGrowth = 0f;
		[SerializeField] private bool ShootMute;



		private float elpasedTime;
		private float nextShootTime;

		private AudioSource ShootSound;

		private void Start()
		{
			Init();
		}

		private void Update()
		{
			DoAction?.Invoke();
			
		}

		public override void Init()
		{
			ShootSound = GetComponent<AudioSource>();
			elpasedTime = FireRate;
			SetNextShootTime();
			SetModeSpin();
		}


		private void SetModeSpin()
		{
			DoAction = DoActionSpin;
		}

		private void DoActionSpin()
		{
			float isClockWise = ((RotationIsClockwise) ? -1 : 1 );
			bool inGround = CheckInGround();
			transform.Rotate(0, 0, isClockWise * RotationSpeed * Time.deltaTime);

			elpasedTime += Time.deltaTime;

			if (elpasedTime >= nextShootTime && !inGround)
			{
				elpasedTime = 0;
				SetNextShootTime();
				for (int i = 0; i < ShootPerFire; i++)
				{
					Shoot();
				}

			}
			else if(inGround) elpasedTime = nextShootTime;


		}

	


		private void Shoot()
		{
			
			Quaternion rotation = Quaternion.AngleAxis(Random.Range(-ShootAngleNoiseMinus, ShootAngleNoisePlus), Vector3.forward) * canon.rotation;
			float speed = ShotSpeed + Random.Range(-ShootSpeedNoiseMinus, ShootSpeedNoisePlus);

			Instantiate(shootPrefab, shootSpawn.position, rotation).Init(speed, ShootLifeTime, ShotFriction, ShootSize, ShootGrowth, ShootMute);

			if (ShootSound.isPlaying) return;
			ShootSound.Play();
		}

		

		private bool CheckInGround()
		{
			return Physics2D.Raycast(transform.position, transform.right, DetectGroundDistance, GroundLayer);
		}

		private void SetNextShootTime()
		{
			nextShootTime = FireRate + Random.Range(-FireRateNoise, FireRateNoise);
		}
	}
}