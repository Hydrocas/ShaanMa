///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 26/02/2020 23:12
///-----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.Common.Audio.Emitter {
	public class AleaAudioEmitter : AudioEmitter {

		[SerializeField] private float frequency = 1;
		[SerializeField] private float frequencyVariance = 0;

		private List<SoundTag> soundsToPlay;

		private Coroutine playCoroutine;

		private void Start()
		{
			if (soundsNames.Length == 0) return;

			soundsToPlay = new List<SoundTag>(SoundsNames);

			if (auto) return;

			playCoroutine = StartCoroutine(WaitToPlayRoutine());
		}

		private IEnumerator WaitToPlayRoutine()
		{
			Play();

			yield return new WaitForSeconds(Random.Range(frequency - frequencyVariance, frequency + frequencyVariance));

			playCoroutine = StartCoroutine(WaitToPlayRoutine());
		}

		protected void Play()
		{
			if (soundsToPlay.Count == 0)
				soundsToPlay = new List<SoundTag>(SoundsNames);

			SoundTag soundName = soundsToPlay[Random.Range(0, soundsToPlay.Count)];
			soundsToPlay.Remove(soundName);

			Play(soundName);
		}

		public void PlayContinues()
		{
			Stop();
			playCoroutine = StartCoroutine(WaitToPlayRoutine());
		}

		public void Stop()
		{
			if (playCoroutine != null) 
				StopCoroutine(playCoroutine);
		}
	}
}