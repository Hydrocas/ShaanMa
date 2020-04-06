
using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects {
	public class ScrollingLayer : MonoBehaviour {

        [SerializeField] private float followRatio0;
        [SerializeField] private float followRatio1;
        [SerializeField] private Transform cameraToFollow;
        private Vector3 lastCameraPosition;
        private Vector3 movementSinceLastFrame;


        // Start is called before the first frame update
        private void Start()
        {
            lastCameraPosition = cameraToFollow.position;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            movementSinceLastFrame = cameraToFollow.position - lastCameraPosition;
            transform.GetChild(0).transform.position += movementSinceLastFrame * followRatio0;
            transform.GetChild(1).transform.position += movementSinceLastFrame * followRatio1;
            lastCameraPosition = cameraToFollow.position;
        }

    }
}