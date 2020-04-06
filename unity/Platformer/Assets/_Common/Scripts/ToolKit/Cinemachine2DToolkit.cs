///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/01/2020 14:41
///-----------------------------------------------------------------

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.Common.ToolKit {
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class Cinemachine2DToolkit : MonoBehaviour {

		private CinemachineVirtualCamera virtualCamera;
		private Coroutine shakeCoroutine;
		private CinemachineBasicMultiChannelPerlin multiChannelPerlin;
		private Vector2[] noiseStartAmplitudes;

		private NoiseSettings noiseSettings;

		[SerializeField] private float shakeDuration = 1;
		[SerializeField] private float shakeAmplitude = 1;
		[SerializeField] private float shakeFrequency = 1;
		[SerializeField] private AnimationCurve shakeCurve;
		[SerializeField] private Vector2 orientation;

		[SerializeField] private bool shakeButton;

		private void OnValidate()
		{
			if (shakeButton)
			{
				Shake(shakeDuration, shakeAmplitude, shakeFrequency, orientation);
				shakeButton = false;
			}
		}

		private void Start()
		{
			virtualCamera = GetComponent<CinemachineVirtualCamera>();
			multiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			
			NoiseSettings.TransformNoiseParams[] transformNoiseParamsList = multiChannelPerlin.m_NoiseProfile.PositionNoise;
			NoiseSettings noiseSettings = new NoiseSettings();
			//noiseSettings.PositionNoise = NoiseSettings.TransformNoiseParams[];

			/*
			noiseStartAmplitudes = new Vector2[transformNoiseParamsList.Length];
			NoiseSettings.TransformNoiseParams transformNoiseParams;


			for (int i = transformNoiseParamsList.Length - 1; i >= 0; i--)
			{
				transformNoiseParams = transformNoiseParamsList[i];
				noiseStartAmplitudes[i] = new Vector2(transformNoiseParams.X.Amplitude, transformNoiseParams.Y.Amplitude);
				Debug.Log(noiseStartAmplitudes[i]);
			}
			*/
		}

		public void Shake(float shakeDuration, float shakeAmplitude, float shakeFrequency, Vector2 orientation)
		{
			ShakeOrientation(orientation);
			Shake(shakeDuration, shakeAmplitude, shakeFrequency);
		}

		public void Shake(float shakeDuration, float shakeAmplitude, float shakeFrequency)
		{
			if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);

			shakeCoroutine = StartCoroutine(ShakeCoroutine(shakeDuration, shakeAmplitude, shakeFrequency));
		}

		private IEnumerator ShakeCoroutine(float shakeDuration, float shakeAmplitude, float shakeFrequency)
		{
			float elapsedTime = 0;
			float coef;

			multiChannelPerlin.m_FrequencyGain = shakeFrequency;

			while (elapsedTime < shakeDuration)
			{
				elapsedTime += Time.deltaTime;
				coef = shakeCurve.Evaluate(elapsedTime / shakeDuration);
				multiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(shakeAmplitude, 0, coef);

				yield return null;
			}

			ShakeOrientation(Vector2.one);
			shakeCoroutine = null;
		}

		private void ShakeOrientation(Vector2 orientation)
		{
			NoiseSettings.TransformNoiseParams[] transformNoiseParamsList = multiChannelPerlin.m_NoiseProfile.PositionNoise;
			NoiseSettings.TransformNoiseParams transformNoiseParams;

			orientation.x = Mathf.Clamp(orientation.x, 0, 1);
			orientation.y = Mathf.Clamp(orientation.y, 0, 1);

			for (int i = transformNoiseParamsList.Length - 1; i >= 0; i--)
			{
				transformNoiseParams = transformNoiseParamsList[i];
				
				transformNoiseParams.X.Amplitude = noiseStartAmplitudes[i].x * orientation.x;
				transformNoiseParams.Y.Amplitude = noiseStartAmplitudes[i].y * orientation.y;

				transformNoiseParamsList[i] = transformNoiseParams;
			}

			multiChannelPerlin.m_NoiseProfile.PositionNoise = transformNoiseParamsList;
		}
	}
}