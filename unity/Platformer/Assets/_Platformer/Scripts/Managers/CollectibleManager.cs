///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 21/01/2020 11:36
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using Com.Isartdigital.Platformer.LevelObjects;
using UnityEngine;

namespace Com.Isartdigital.Platformer.Managers {
	public class CollectibleManager {

        public event Action<CollectibleManager, float> OnCollectibleCollected;

        private List<Collectible> collectibles;
        private int count;

        public int Count => count;

        public CollectibleManager(List<Collectible> collectibles) {
            this.collectibles = collectibles;

            for (int i = this.collectibles.Count - 1; i >= 0; i--) {
                this.collectibles[i].OnCollected += Collectible_OnCollected;
            }
        }

        private void Collectible_OnCollected(Collectible obj,float timeEarned) {
            count++;
            OnCollectibleCollected?.Invoke(this, timeEarned);
        }
    }
}