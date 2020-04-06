///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 17/03/2020 15:53
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Com.Isartdigital.Common.UI {
	public class SoundVolumeSlider : MonoBehaviour {

		[SerializeField] private AudioMixer audioMixer;
		[SerializeField] private string paramName = "Volume";

		private void Awake()
		{
			Slider slider = GetComponentInChildren<Slider>();
			slider.onValueChanged.AddListener(Slider_OnValueChanged);
		}

		private void Slider_OnValueChanged(float value)
		{
			audioMixer.SetFloat(paramName, value);
		}
	}
}