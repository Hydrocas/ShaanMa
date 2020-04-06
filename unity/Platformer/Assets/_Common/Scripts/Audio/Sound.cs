///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 30/11/2019 00:27
///-----------------------------------------------------------------

using UnityEngine;
using System;
using UnityEngine.Audio;

namespace Com.Isartdigital.Common.Audio {

    [Serializable]
	public class Sound {

        [SerializeField] private string tag;

        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioMixerGroup mixer;
        [SerializeField, Range(0f, 1f)] private float volume = 1f;
        [SerializeField, Range(0.1f, 3f)] private float pitch = 1f;
        [SerializeField] private bool loop;
        [SerializeField] private SoundType type;

        [HideInInspector] public bool showInfoEditor;

        private AudioSource src;

        public AudioSource Source {
            set
            {
                src = value;

                src.clip = clip;
                src.volume = Volume;
                src.pitch = pitch;
                src.loop = Loop;

                if (tag == "")
                    tag = clip.name;
            }
            get => src;
        }

        public string Tag => tag;
        public float Volume => volume;
        public SoundType Type => type;

        public bool Loop => loop;
    }
}