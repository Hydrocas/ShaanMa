///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 20/03/2020 14:33
///-----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.Common.Audio.Emitter {
	public class AleaAudioSource : MonoBehaviour {

		[SerializeField] private float frequency = 1;
		[SerializeField] private float frequencyVariance = 0;
		[SerializeField] private bool auto;
		private AudioSource[] sounds;

		private List<AudioSource> soundsToPlay;
		private bool isPlaying;
		private Coroutine playCoroutine;

		public bool IsPlaying => isPlaying;

		private void Start()
		{
			sounds = GetComponentsInChildren<AudioSource>();
			soundsToPlay = new List<AudioSource>(sounds);

			if (auto) return;

			playCoroutine = StartCoroutine(WaitToPlayRoutine());
		}

		private IEnumerator WaitToPlayRoutine()
		{
			isPlaying = true;
			Play();

			yield return new WaitForSeconds(Random.Range(frequency - frequencyVariance, frequency + frequencyVariance));

			playCoroutine = StartCoroutine(WaitToPlayRoutine());
		}

		protected void Play()
		{
			if (soundsToPlay.Count == 0)
				soundsToPlay = new List<AudioSource>(sounds);

			AudioSource sound = soundsToPlay[Random.Range(0, soundsToPlay.Count)];
			soundsToPlay.Remove(sound);

			sound.Play();
		}

		public void PlayContinues()
		{
			Stop();
			playCoroutine = StartCoroutine(WaitToPlayRoutine());
		}

		public void Stop()
		{
			if (playCoroutine == null) return;
			
			StopCoroutine(playCoroutine);
			isPlaying = false;
		}
	}
}