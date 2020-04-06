///-----------------------------------------------------------------
/// Author : Maximilien Galea et Jacob Loïc
/// Date : 21/01/2020 12:02
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.LevelObjects.Obstacles;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects {
    public class Level : MonoBehaviour {

        [SerializeField] private GameObject checkpointContainer;
        [SerializeField] private GameObject collectibleContainer;
        [SerializeField] private GameObject doorContainer;
        [SerializeField] private ScrollingLayer[] _scrollingLayers;
        [SerializeField] private Goal _goal;
        [SerializeField] private float _timeLimit;

        private List<Checkpoint> _checkpointList = new List<Checkpoint>();
        private List<Collectible> _collectibleList = new List<Collectible>();
        private List<TriggerDoor> _doorList = new List<TriggerDoor>();

        public List<Checkpoint> CheckpointList => _checkpointList;
        public List<Collectible> CollectibleList => _collectibleList;
        public List<TriggerDoor> DoorList => _doorList;
        public ScrollingLayer[] ScrollingLayers => _scrollingLayers;
        public Goal Goal => _goal;
        public float TimeLimit => _timeLimit;

        private void Awake()
        {

            StackItemsInLists(checkpointContainer, _checkpointList);
            StackItemsInLists(collectibleContainer, _collectibleList);
            StackItemsInLists(doorContainer, _doorList);
        }

        private void StackItemsInLists<T>(GameObject objectContainer, List<T> objectList)
        {
            
            for (int i = 0; i < objectContainer.transform.childCount; i++)
                objectList.Add( objectContainer.transform.GetChild(i).GetComponent<T>() );
        }

    }
}