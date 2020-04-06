///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 19/01/2020 17:07
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.PlayerScript {
    [CreateAssetMenu(fileName ="New Player Particle Settings",menuName ="Player/ParticleSettings")]
	public class PlayerParticleSettings : ScriptableObject {
        [SerializeField] private ParticleSystem _landing;
        [SerializeField] private ParticleSystem _wallSliding;
        [SerializeField] private ParticleSystem _sprintOnDirt;

        public ParticleSystem Landing => _landing;

        public ParticleSystem WallSliding => _wallSliding;
        public ParticleSystem SprintOnDirt => _sprintOnDirt;
    }
}