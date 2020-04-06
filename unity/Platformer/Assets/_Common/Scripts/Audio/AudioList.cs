///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 21/02/2020 10:59
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Com.Isartdigital.Common.Audio {
    [CreateAssetMenu(fileName = "New Audio Settings", menuName = "Audio/Settings")]
	public class AudioList : ScriptableObject {

        [SerializeField] private AudioMixerGroup globalMixer;
        [SerializeField] private Sound[] sounds;

        private const string ENUM_NAME = "SoundTag";
        private const string DEFAULT_TAG = "None";

        public AudioMixerGroup GlobalMixer => globalMixer;
        public Sound[] Sounds => sounds;

        public void GenerateTags()
        {
#if UNITY_EDITOR
            List<string> tags = new List<string>() { DEFAULT_TAG };

            for (int i = sounds.Length - 1; i >= 0; i--)
            {
                tags.Add(sounds[i].Tag);
            }

            EnumGenerator.Write("Assets/_Platformer/Scripts/Enums/", ENUM_NAME, tags.ToArray());
#endif
        }
    }
}