using Com.Isartdigital.Common.Audio;
using Com.IsartDigital.Common.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToolkit : MonoBehaviour
{
	private Dictionary<string, ParticleSystem> _particleDico = new Dictionary<string, ParticleSystem>();


    void Start()
    {
		ParticleSystem[] foundParticles = GetComponentsInChildren<ParticleSystem>(true);

		for (int i = 0; i < foundParticles.Length; i++)
		{
			if(!_particleDico.ContainsKey(foundParticles[i].name))
				_particleDico.Add(foundParticles[i].name, foundParticles[i]);
		}
    }


	public void PlayParticle(string particleName)
	{
		if (_particleDico.ContainsKey(particleName))
			_particleDico[particleName].Play();
	}

    public void PlaySound(SoundTag name)
    {
        AudioManager.Instance.Play(name);
    }

}
