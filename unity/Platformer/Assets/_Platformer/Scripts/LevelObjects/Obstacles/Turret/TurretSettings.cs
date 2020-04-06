///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 04/02/2020 17:33
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects.Obstacles.Turret {
	
	[CreateAssetMenu(
		menuName = "Obstacle/Turret",
		fileName = "TurretConfig",
		order = 0
	)]
	
	public class TurretSettings : ScriptableObject {

		[SerializeField] private float shootSpeed = 1;
		[SerializeField] private float shootFriction = 1;
		[SerializeField] private float rotationSpeed = 1;

		[SerializeField] private float distanceDetection = 10;
		[SerializeField] private float angleDetection = 30;
		[SerializeField] private float defaultAngle = 90;

		[SerializeField] private float fireRate;
		[SerializeField] private float shootAngleNoise = 0;
		[SerializeField] private float shootSpeedNoise = 0;
		[SerializeField] private float fireRateNoise = 0;
		[SerializeField] private float shootLifeTime = 6;

		public float ShootSpeed => shootSpeed;
		public float ShootFriction => shootFriction;
		public float DistanceDetection => distanceDetection;
		public float DefaultAngle => defaultAngle;
		public float RotationSpeed => rotationSpeed;
		public float AngleDetection => angleDetection;
		public float FireRate => fireRate;
		public float ShootAngleNoise => shootAngleNoise;
		public float ShootSpeedNoise => shootSpeedNoise;
		public float FireRateNoise => fireRateNoise;
		public float ShootLifeTime => shootLifeTime;
	}
}