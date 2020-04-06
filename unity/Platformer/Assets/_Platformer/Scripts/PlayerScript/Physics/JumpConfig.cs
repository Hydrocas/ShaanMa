///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 03/03/2020 14:57
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.PlayerScript.Physics {
	public struct JumpConfig {

		public AnimationCurve verticalCurve;
		public float verticalDistance;

		public AnimationCurve horizontalCurve;
		public float horizontalDistance;

		public float jumpTime;

		public JumpConfig(float time, AnimationCurve vCurve, float vDistance)
		{
			verticalCurve = vCurve;
			verticalDistance = vDistance;

			horizontalCurve = null;
			horizontalDistance = 0;

			jumpTime = time;
		}

		public JumpConfig(float time, AnimationCurve vCurve, float vDistance, AnimationCurve hCurve, float hDistance)
		{
			verticalCurve = vCurve;
			verticalDistance = vDistance;

			horizontalCurve = hCurve;
			horizontalDistance = hDistance;

			jumpTime = time;
		}

		public JumpConfig(JumpConfig jumpConfig)
		{
			verticalCurve = jumpConfig.verticalCurve;
			verticalDistance = jumpConfig.verticalDistance;

			horizontalCurve = jumpConfig.horizontalCurve;
			horizontalDistance = jumpConfig.horizontalDistance;

			jumpTime = jumpConfig.jumpTime;
		}

		public Vector2 CalcEndPos(Vector2 startPos)
		{
			return startPos + new Vector2(horizontalDistance, verticalDistance);
		}

	}
}