///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 25/02/2020 19:32
///-----------------------------------------------------------------

using Com.Isartdigital.Common.Audio.Transition;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.Common.Audio.Emitter {
	public class AudioEmitter : MonoBehaviour {

		[SerializeField] protected AudioTransitionConfig transitionConfig;
		[SerializeField] protected SoundTag[] soundsNames;
		[SerializeField] protected bool auto;

		public SoundTag[] SoundsNames => soundsNames;
		public AudioTransitionConfig TransitionConfig => transitionConfig;

		private void Awake()
		{
			List<SoundTag> soundTags = new List<SoundTag>();
			SoundTag soundTag;

			for (int i = soundsNames.Length - 1; i >= 0; i--)
			{
				soundTag = soundsNames[i];

				if (!soundTags.Contains(soundTag))
					soundTags.Add(soundTag);
			}

			soundsNames = soundTags.ToArray();
		}

		private void Start()
		{
			if (!auto) return;

			TransitionAll();
		}

		public void TransitionAll()
		{
			AudioManager.Instance.Transition(SoundsNames, TransitionConfig);
		}

		protected virtual void Play(SoundTag name)
		{
			AudioManager.Instance.Play(name);
		}

		protected AudioEmitter()
		{
			transitionConfig = AudioTransitionConfig.Default;
		}
	}
}