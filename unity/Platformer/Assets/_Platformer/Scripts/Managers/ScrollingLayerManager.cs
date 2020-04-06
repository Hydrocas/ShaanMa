///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 21/01/2020 11:37
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.LevelObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.Platformer.Managers {
	public class ScrollingLayerManager : MonoBehaviour {
        [SerializeField] private Transform cameraToFollow;
        [Header("La list follow ratio doit être égale au nombre de children")]
        [SerializeField] List<float> FollowRatio;
        private List<Transform> scrollingLayers;
        private int childrenNumber;
        private Vector3 lastCameraPosition;
        private Vector3 movementSinceLastFrame;

        public ScrollingLayerManager(List<Transform> scrollingLayers) {
            this.scrollingLayers = scrollingLayers;
        }

        private void Start()
        {
            scrollingLayers = new List<Transform>();

            childrenNumber = transform.childCount;
            if (FollowRatio.Count != childrenNumber) {
                Terminate();
                return;
            };

            for (int i = 0; i <= childrenNumber - 1; i++)
            {
                scrollingLayers.Add(transform.GetChild(i));
            }
            lastCameraPosition = cameraToFollow.position;
        }

        private void FixedUpdate()
        {
            movementSinceLastFrame = cameraToFollow.position - lastCameraPosition;
            for (int i = 0; i <= childrenNumber - 1; i++)
            {
                scrollingLayers[i].transform.position += movementSinceLastFrame * FollowRatio[i];
            }
            lastCameraPosition = cameraToFollow.position; 
        }

        private void Terminate()
        {
            Debug.LogError("Number of children different from number of followratio given. They must be the same number.");

            this.gameObject.SetActive(false);

        }
    }
}