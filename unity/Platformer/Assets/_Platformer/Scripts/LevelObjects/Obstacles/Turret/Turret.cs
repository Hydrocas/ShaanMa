///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 04/02/2020 16:17
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Objects;
using System.Collections;
using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects.Obstacles.Turret {
	public class Turret : StateObject {

		[SerializeField] private Transform canon;
		[SerializeField] private Transform shootSpawn;
		[SerializeField] private Shoot shootPrefab;
		[SerializeField] private Transform target;
		[SerializeField] private TurretSettings settings;

		private float elpasedTime;
		private float nextShootTime;

		private void Start()
		{
			canon.rotation = Quaternion.AngleAxis(settings.DefaultAngle, Vector3.forward);
			Init();
		}

		private void Update()
		{
			DoAction?.Invoke();
		}

		public override void Init()
		{
			SetNextShootTime();
			SetModeWait();
		}

		private void SetModeWait()
		{
			DoAction = DoActionWait;
		}

		private void DoActionWait()
		{
			if (CheckTarget()) SetModeTarget();
		}

		private void SetModeTarget()
		{
			DoAction = DoActionTarget;
		}

		private void DoActionTarget()
		{
			Vector2 toTarger = target.position - transform.position;
			Quaternion targetRotation = Quaternion.AngleAxis(Mathf.Atan2(toTarger.y, toTarger.x) * Mathf.Rad2Deg, Vector3.forward);

			canon.rotation = Quaternion.RotateTowards(canon.rotation, targetRotation, settings.RotationSpeed * Time.deltaTime);

			elpasedTime += Time.deltaTime;

			if(elpasedTime >= nextShootTime)
			{
				elpasedTime -= nextShootTime;
				SetNextShootTime();
				Shoot();
			}

			if (!CheckTarget())
			{
				elpasedTime = 0;
				SetModeGoIdle();
			}
		}

		private void SetModeGoIdle()
		{
			DoAction = DoActionGoIdle;
		}

		private void DoActionGoIdle()
		{
			Quaternion targetRotation = Quaternion.AngleAxis(settings.DefaultAngle, Vector3.forward);

			canon.rotation = Quaternion.RotateTowards(canon.rotation, targetRotation, settings.RotationSpeed * Time.deltaTime);

			if (CheckTarget()) SetModeTarget();
			else if (Mathf.Approximately(canon.eulerAngles.z, settings.DefaultAngle)) SetModeWait();
		}

		private void Shoot()
		{
			Quaternion rotation = Quaternion.AngleAxis(Random.Range(-settings.ShootAngleNoise, settings.ShootAngleNoise), Vector3.forward) * canon.rotation;
			float speed = settings.ShootSpeed + Random.Range(-settings.ShootSpeedNoise, settings.ShootSpeedNoise);

			//Instantiate(shootPrefab, shootSpawn.position, rotation).Init(speed, settings.ShootLifeTime, settings.ShootFriction);
		}

		private bool CheckTarget()
		{
			return Vector2.Distance(transform.position, target.position) <= settings.DistanceDetection 
				&& Vector2.Angle(transform.right, target.position - transform.position) < settings.AngleDetection;
		}

		private void SetNextShootTime()
		{
			nextShootTime = settings.FireRate + Random.Range(-settings.FireRateNoise, settings.FireRateNoise);
		}
	}
}