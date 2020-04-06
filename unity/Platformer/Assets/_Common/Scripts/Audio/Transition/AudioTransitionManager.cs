///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 26/02/2020 15:21
///-----------------------------------------------------------------

using Com.Isartdigital.Common.Audio.Emitter;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.Common.Audio.Transition {
	public class AudioTransitionManager : MonoBehaviour {

		[SerializeField] private List<AudioEmitter> list = new List<AudioEmitter>();

		private SoundTag[] lastSoundsNames;

		public void Transition(AudioEmitter audioEmitter)
		{
			if (lastSoundsNames == null) 
			{
				AudioManager.Instance.Play(audioEmitter.SoundsNames, audioEmitter.TransitionConfig);
			}
			else
			{
				AudioManager.Instance.Transition(lastSoundsNames, audioEmitter.SoundsNames, audioEmitter.TransitionConfig);
			}

			audioEmitter.gameObject.SetActive(false);
			lastSoundsNames = audioEmitter.SoundsNames;
		}
	}
}