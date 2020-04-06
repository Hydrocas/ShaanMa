///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 28/01/2020 17:18
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.PlayerScript.Physics {
	[CreateAssetMenu(fileName = "New Raycast settings", menuName = "Player/RaycastSettings")]
	public class RaycastSettings : ScriptableObject {

		[Header("Mask Raycast")]
		[SerializeField] private LayerMask _raycastMask;

		[Header("Ground Raycast")]
		[SerializeField] private float _groundedRaycastDistance;
		[SerializeField] private float _groundedRaycastTolerance;
		[SerializeField, Range(1, 5)] private int _groundedRaycastNumber;

		[Header("Air Ground Raycast")]
		[SerializeField] private float _airGroundedRaycastDistance;
		[SerializeField] private float _airGroundedRaycastTolerance;
		[SerializeField, Range(1, 5)] private int _airGroundedRaycastNumber;
		[SerializeField] private float _airGroundSnapDistance;

		[Header("Head Raycast")]
		[SerializeField] private float _headRaycastDistance;
		[SerializeField] private float _headRaycastTolerance;
		[SerializeField, Range(1, 5)] private int _headRaycastNumber;

		[Header("Side Raycast")]
		[SerializeField] private float _sideRaycastDistance;
		[SerializeField] private float _sideRaycastTolerance;
		[SerializeField, Range(1, 5)] private int _sideRaycastNumber;
		[SerializeField] private float _sideSnapDistance;

		public LayerMask RaycastMask => _raycastMask;

		public float GroundedRaycastDistance => _groundedRaycastDistance;
		public float GroundedRaycastTolerance => _groundedRaycastTolerance;
		public int GroundedRaycastNumber => _groundedRaycastNumber;

		public float HeadRaycastDistance => _headRaycastDistance;
		public float HeadRaycastTolerance => _headRaycastTolerance;
		public int HeadRaycastNumber => _headRaycastNumber;

		public float SideRaycastDistance => _sideRaycastDistance;
		public float SideRaycastTolerance => _sideRaycastTolerance;
		public int SideRaycastNumber => _sideRaycastNumber;

		public float AirGroundedRaycastDistance => _airGroundedRaycastDistance;
		public float AirGroundedRaycastTolerance => _airGroundedRaycastTolerance;
		public int AirGroundedRaycastNumber => _airGroundedRaycastNumber;

		public float AirGroundSnapDistance => _airGroundSnapDistance;
		public float SideSnapDistance => _sideSnapDistance;
	}
}