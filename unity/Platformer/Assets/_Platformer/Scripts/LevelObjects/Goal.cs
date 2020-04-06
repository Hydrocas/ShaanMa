///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 21/01/2020 12:25
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects {
    [RequireComponent(typeof(Collider2D))]
	public class Goal : MonoBehaviour {

        public event Action<Goal> OnTrigger;

        private void OnTriggerEnter2D(Collider2D collision) {
            OnTrigger?.Invoke(this);
        }
    }
}