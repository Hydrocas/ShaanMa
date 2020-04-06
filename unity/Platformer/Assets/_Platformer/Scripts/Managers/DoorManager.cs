///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 20/02/2020 12:09
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.LevelObjects.Obstacles;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.Platformer.Managers {
	public class DoorManager {
        private List<TriggerDoor> doors;
        private List<TriggerDoor> doorToReset;
        private List<Lever> leverToReset;

        public DoorManager(List<TriggerDoor> doors) {
            this.doors = new List<TriggerDoor>();
            leverToReset = new List<Lever>();

            this.doors = doors;

            for (int i = doors.Count - 1; i >= 0; i--) {
                doors[i].OnTrigger += DoorManager_OnTrigger;
            }

            doorToReset = new List<TriggerDoor>();
        }

        private void DoorManager_OnTrigger(TriggerDoor door, Lever lever) {
            doorToReset.Add(door);
            leverToReset.Add(lever);
        }

        public void ResetDoors() {
            Debug.Log(doorToReset.Count);

            for (int i = doorToReset.Count - 1; i >= 0; i--) {
                doorToReset[i].ResetPosition();
            }

            for (int j = leverToReset.Count - 1; j >= 0; j--) {
                leverToReset[j].ResetPosition();
            }
            ClearListToReset();
        }

        public void DoorManager_OnCheckpointPassed() {
            ClearListToReset();
        }

        private void ClearListToReset() {
            doorToReset.Clear();
            leverToReset.Clear();
        }
    }
}