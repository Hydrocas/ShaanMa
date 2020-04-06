///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 25/02/2020 15:35
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.Isartdigital.Common.Audio.Transition {

	[Serializable]
	public class AudioTransitionConfig {

		public float duration = 1;
		public float offset = 1;
		public AnimationCurve curve;
		public SoundTag transitionSound;

		public static AudioTransitionConfig Default { 
			get
			{
				return new AudioTransitionConfig(1, 1);
			}
		}

		public AudioTransitionConfig(float duration, float offset, SoundTag transitionSound, AnimationCurve curve = null) : this(duration, offset)
		{
			this.transitionSound = transitionSound;
			this.curve = curve ?? AnimationCurve.Linear(0, 0, 1, 1);
		}

		public AudioTransitionConfig(float duration, float offset)
		{
			this.duration = duration;
			this.offset = offset;
		}
	}
}