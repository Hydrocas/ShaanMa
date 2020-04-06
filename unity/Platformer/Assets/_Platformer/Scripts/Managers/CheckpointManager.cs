///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 21/01/2020 11:37
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using Com.Isartdigital.Platformer.LevelObjects;
using UnityEngine;

namespace Com.Isartdigital.Platformer.Managers {
    public class CheckpointManager {

        private List<Checkpoint> checkpoints;
        private Vector2 _lastCheckpointPos;
        public Vector2 LastCheckpointPos => _lastCheckpointPos;

        public event Action OnCheckpointPassed;

        public CheckpointManager( List<Checkpoint> checkpoints) {
            this.checkpoints = checkpoints;
            for (int i = checkpoints.Count - 1; i >= 0; i--) {
                checkpoints[i].onTrigger += Checkpoint_OnTrigger;
            }

        }


        private void Checkpoint_OnTrigger(Checkpoint checkpoint) {
            OnCheckpointPassed?.Invoke();
            _lastCheckpointPos = checkpoint.transform.position;
        }
    }
}