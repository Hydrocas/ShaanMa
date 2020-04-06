///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 23/02/2020 00:52
///-----------------------------------------------------------------

using UnityEngine;
using System;

namespace Com.Isartdigital.Common {
	public class HoverLerp : MonoBehaviour {

		[Header("positions")]
		[SerializeField] private Transform from;
		[SerializeField] private Transform to;

		[Header("Animation Param")]
		[SerializeField] private AnimationCurve curve;
		[SerializeField] private float time;

		private float elapsedTime;

		private void Start()
		{
			elapsedTime = UnityEngine.Random.Range(0, time);
		}

		private void Update()
		{
			elapsedTime += Time.deltaTime;

			float coef = curve.Evaluate(elapsedTime / time);

			transform.position = Vector3.LerpUnclamped(from.position, to.position, coef);
		}
	}
}