///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 28/01/2020 17:12
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.PlayerScript.Physics {
	public class PlayerRaycast : MonoBehaviour {

		[SerializeField] private bool isDebug;
		[SerializeField] private RaycastSettings settings;

		[SerializeField] private Transform startRaycastGround;
		[SerializeField] private Transform endRaycastGround;

		[SerializeField] private Transform startRaycastHead;
		[SerializeField] private Transform endRaycastHead;

		[SerializeField] private Transform startRaycastSide;
		[SerializeField] private Transform endRaycastSide;
		[SerializeField] private Transform rightRaycastSide;
		[SerializeField] private Transform leftRaycastSide;

		private LayerMask mask;

		private void Awake()
		{
			mask = settings.RaycastMask;
		}

		#region Ground

		public RaycastHit2D GroundRaycasts
		{
			get
			{
				return DoRaycasts(-transform.up, startRaycastGround.position, endRaycastGround.position,
					settings.GroundedRaycastTolerance, settings.GroundedRaycastDistance,
					settings.GroundedRaycastNumber, Color.blue);
					
			}
		}

		public RaycastHit2D AirGroundRaycasts
		{
			get
			{
				return DoRaycasts(-transform.up, startRaycastGround.position, endRaycastGround.position,
					settings.AirGroundedRaycastTolerance, settings.AirGroundedRaycastDistance,
					settings.AirGroundedRaycastNumber, Color.cyan);

			}
		}

		public RaycastHit2D AirGroundRaycastsSnap
		{
			get
			{
				return DoRaycasts(-transform.up, startRaycastGround.position, endRaycastGround.position,
					settings.AirGroundSnapDistance, settings.AirGroundedRaycastDistance,
					settings.AirGroundedRaycastNumber, Color.cyan);

			}
		}

        #endregion

        #region Head

        public RaycastHit2D HeadRaycasts
		{
			get
			{
				return DoRaycasts(transform.up, startRaycastHead.position, endRaycastHead.position,
					settings.HeadRaycastTolerance, settings.HeadRaycastDistance,
					settings.HeadRaycastNumber, Color.red);
			}
		}

        #endregion

        #region SideRight

        public RaycastHit2D SideRightRaycasts
		{
			get
			{
				return DoRaycasts(transform.right, new Vector2(rightRaycastSide.position.x, startRaycastSide.position.y),
					new Vector2(rightRaycastSide.position.x, endRaycastSide.position.y),
					settings.SideRaycastTolerance, settings.SideRaycastDistance,
					settings.SideRaycastNumber, Color.green);
			}
		}

		public RaycastHit2D SideRightRaycastsSnap
		{
			get
			{
				return DoRaycasts(transform.right, new Vector2(rightRaycastSide.position.x, startRaycastSide.position.y),
					new Vector2(rightRaycastSide.position.x, endRaycastSide.position.y),
					settings.SideSnapDistance, settings.SideRaycastDistance,
					settings.SideRaycastNumber, Color.green);
			}
		}

        #endregion

        #region SideLeft

        public RaycastHit2D SideLeftRaycasts
		{
			get
			{
				return DoRaycasts(-transform.right, new Vector2(leftRaycastSide.position.x, startRaycastSide.position.y),
					new Vector2(leftRaycastSide.position.x, endRaycastSide.position.y),
					settings.SideRaycastTolerance, settings.SideRaycastDistance,
					settings.SideRaycastNumber, Color.yellow);
			}
		}

		public RaycastHit2D SideLeftRaycastsSnap
		{
			get
			{
				return DoRaycasts(-transform.right, new Vector2(leftRaycastSide.position.x, startRaycastSide.position.y),
					new Vector2(leftRaycastSide.position.x, endRaycastSide.position.y),
					settings.SideSnapDistance, settings.SideRaycastDistance,
					settings.SideRaycastNumber, Color.yellow);
			}
		}

        #endregion

        private RaycastHit2D DoRaycasts(Vector2 direction, Vector2 startPos, Vector2 endPos, float tolerence, float distance, int number, Color debugColor)
		{
			Vector2 origin;
			RaycastHit2D hit = new RaycastHit2D();

			for (int i = 0; i < number; i++)
			{
				origin = Vector2.Lerp(endPos, startPos, (float)i / (float)(number - 1)) - (direction * distance);
				if (isDebug) Debug.DrawRay(origin, direction * (distance + tolerence), debugColor);

				hit = Physics2D.Raycast(origin, direction, distance + tolerence, mask);
				if(hit) return hit;
			}

			return hit;
		}
	}
}