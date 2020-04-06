///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 30/11/2019 00:26
///-----------------------------------------------------------------

using Com.Isartdigital.Common.Audio.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.Common.Audio
{

    public enum SoundType
    {
        Asynchrone,
        Synchrone
    }

	public class AudioManager : MonoBehaviour {

        private static AudioManager instance;
        public static AudioManager Instance { get => instance; }

        private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();
        private List<Sound> playingSounds;
        
        [SerializeField] private AudioList list;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            foreach (Sound sound in list.Sounds)
            {
                sound.Source = gameObject.AddComponent<AudioSource>();

                sound.Source.outputAudioMixerGroup = list.GlobalMixer;
                soundDictionary.Add(sound.Tag, sound);

                if (sound.Type != SoundType.Synchrone) continue;
                
                sound.Source.volume = 0;
                sound.Source.loop = true;
                sound.Source.Play();
            }

            playingSounds = new List<Sound>();
        }

        #region Transition

        public void Transition(SoundTag[] soundsNames, AudioTransitionConfig audioTransitionConfig)
        {
            if(playingSounds.Count == 0)
            {
                Play(soundsNames, audioTransitionConfig);
            }
            else
            {
                Transition(playingSounds.ToArray(), GetSoundsByTags(soundsNames), audioTransitionConfig);
            }
        }

        public void Transition(SoundTag[] prevSoundsNames, SoundTag[] nextSoundsNames, AudioTransitionConfig audioTransitionConfig)
        {
            Transition(GetSoundsByTags(prevSoundsNames), GetSoundsByTags(nextSoundsNames), audioTransitionConfig);
        }

        public void Transition(Sound[] prevSounds, Sound[] nextSounds, AudioTransitionConfig audioTransitionConfig)
        {
            List<Sound> prevSoundsList = new List<Sound>(prevSounds);
            List<Sound> nextSoundsList = new List<Sound>(nextSounds);

            EjectSameSounds(prevSoundsList, nextSoundsList);

            Play(nextSoundsList.ToArray(), audioTransitionConfig);

            audioTransitionConfig.offset = 0;

            Stop(prevSoundsList.ToArray(), audioTransitionConfig);
        }

        #endregion

        #region Play

        public void Play(SoundTag name)
        {
            Sound sound = GetSoundByTag(name);

            if (sound == null) return;
            
            if(!sound.Loop && sound.Type == SoundType.Synchrone)
            {
                StartCoroutine(PlayRoutine(sound));
            }
            else
            {
                sound.Source.Play();
            }
        }

        public void Play(SoundTag[] soundsNames, AudioTransitionConfig transitionConfig)
        {
            Play(GetSoundsByTags(soundsNames), transitionConfig);
        }

        public void Play(Sound[] sounds, AudioTransitionConfig transitionConfig)
        {
            Sound sound;

            for (int i = sounds.Length - 1; i >= 0; i--)
            {
                sound = sounds[i];
                playingSounds.Add(sound);

                StartCoroutine(VolumeRoutine(sound, sound.Source.volume, sound.Volume, transitionConfig));
            }
        }

        private IEnumerator PlayRoutine(Sound sound)
        {
            sound.Source.volume = sound.Volume;

            yield return new WaitForSeconds(sound.Source.clip.length);

            sound.Source.volume = 0;
        }

        #endregion

        #region Stop

        public void Stop(SoundTag name, float duration = 0)
        {
            Sound sound = GetSoundByTag(name);

            if (sound != null) sound.Source.Stop();
        }

        public void Stop(SoundTag[] soundsNames, AudioTransitionConfig transitionConfig)
        {
            Stop(GetSoundsByTags(soundsNames), transitionConfig);
        }

        public void Stop(Sound[] sounds, AudioTransitionConfig transitionConfig)
        {
            Sound sound;

            for (int i = sounds.Length - 1; i >= 0; i--)
            {
                sound = sounds[i];
                playingSounds.Remove(sound);

                StartCoroutine(VolumeRoutine(sound, sound.Source.volume, 0, transitionConfig));
            }
        }

        #endregion

        #region Volume

        private IEnumerator VolumeRoutine(Sound sound, float from, float to, AudioTransitionConfig audioTransitionConfig)
        {
            AudioSource source = sound.Source;
            float elapsedTime = 0;
            float coef;

            if (audioTransitionConfig.offset != 0)
                yield return new WaitForSeconds(audioTransitionConfig.duration * audioTransitionConfig.offset);

            while (elapsedTime < audioTransitionConfig.duration)
            {
                elapsedTime += Time.deltaTime;

                coef = elapsedTime / audioTransitionConfig.duration;

                source.volume = Mathf.Lerp(from, to, audioTransitionConfig.curve.Evaluate(coef));

                yield return null;
            }
        }

        #endregion

        #region Help Methode

        private Sound GetSoundByTag(SoundTag name)
        {
            Sound sound = soundDictionary[name.ToString()];

            if (sound == null) UnfoundedSound(name.ToString());

            return sound;
        }

        private Sound[] GetSoundsByTags(SoundTag[] names)
        {
            Sound[] sounds = new Sound[names.Length];

            for (int i = names.Length - 1; i >= 0; i--)
            {
                sounds[i] = GetSoundByTag(names[i]);

                if (sounds[i] == null) UnfoundedSound(names[i].ToString());
            }

            return sounds;
        }

        private void EjectSameSounds<T>(List<T> prevSounds, List<T> nextSounds)
        {
            for (int i = prevSounds.Count - 1; i >= 0; i--)
            {
                if (nextSounds.Contains(prevSounds[i]))
                {
                    nextSounds.Remove(prevSounds[i]);
                    prevSounds.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Error

        private void UnfoundedSound(string soundName)
        {
            Debug.LogError("Erreur : le son " + soundName + " est introuvable");
        }

        #endregion

        private void OnDestroy()
        {
            if (this == instance) instance = null;
        }
    }
}
 