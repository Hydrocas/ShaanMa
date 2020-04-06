///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 16/03/2020 11:43
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.Common.FX {
	public class OscillerTransform : MonoBehaviour {
		
		//[SerializeField] private bool playOnAwake;
		private List<Action> actions;

		private void Start()
		{
			actions = new List<Action>();

			if (oscilleScale)
				AddModeScale();

			if (oscilleRotation)
				AddModeRotate();

			if (oscillePosition)
				AddModePos();
		}

		private void Update () 
		{
			for (int i = actions.Count - 1; i >= 0; i--)
			{
				actions[i]();
			}
		}

		#region Rotation

		[Space]
		[SerializeField] private bool oscilleRotation;
		[SerializeField] private Vector3 rotationAxe;
		[SerializeField] private float startAngle;
		[SerializeField] private float endAngle;
		[SerializeField] private float timeToRotate;
		[SerializeField] private AnimationCurve rotationCurve;

		private Quaternion startRotation;
		private Quaternion endRotation;
		private float elapsedTimeRotation;

		private void AddModeRotate()
		{
			elapsedTimeRotation = UnityEngine.Random.Range(0f, timeToRotate);
			startRotation = Quaternion.AngleAxis(startAngle, rotationAxe);
			endRotation = Quaternion.AngleAxis(endAngle, rotationAxe);
			actions.Add(DoActionRotate);
		}

		private void DoActionRotate()
		{
			elapsedTimeRotation += Time.deltaTime;
			transform.rotation = Quaternion.Slerp(startRotation, endRotation, rotationCurve.Evaluate(elapsedTimeRotation / timeToRotate));
		}

		#endregion

		#region Position

		[Space]
		[SerializeField] private bool oscillePosition;
		[SerializeField] private Transform startPos;
		[SerializeField] private Transform endPos;
		[SerializeField] private float timeToMove;
		[SerializeField] private AnimationCurve moveCurve;

		private float elapsedTimePos;

		private void AddModePos()
		{
			elapsedTimePos = UnityEngine.Random.Range(0f, timeToMove);
			actions.Add(DoActionPos);
		}

		private void DoActionPos()
		{
			elapsedTimePos += Time.deltaTime;
			transform.position = Vector3.Lerp(startPos.position, endPos.position, moveCurve.Evaluate(elapsedTimePos / timeToMove));
		}

		#endregion

		#region Scale

		[Space]
		[SerializeField] private bool oscilleScale;
		[SerializeField] private Vector3 targetScale;
		[SerializeField] private float timeToScale;
		[SerializeField] private AnimationCurve scaleCurve;

		private Vector3 startScale;
		private float elapsedTimeScale;

		private void AddModeScale()
		{
			elapsedTimeScale = UnityEngine.Random.Range(0f, timeToScale);
			startScale = transform.localScale;
			actions.Add(DoActionScale);
		}

		private void DoActionScale()
		{
			elapsedTimeScale += Time.deltaTime;
			transform.localScale = Vector3.Lerp(startScale, targetScale, scaleCurve.Evaluate(elapsedTimeScale / timeToScale));
		}

        #endregion
    }
}