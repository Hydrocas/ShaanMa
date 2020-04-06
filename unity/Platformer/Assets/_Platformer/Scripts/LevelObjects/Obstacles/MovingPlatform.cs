///-----------------------------------------------------------------
/// Author : Loïc Jacob
/// Date : 27/01/2020 11:21
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects.Obstacles {
	public class MovingPlatform : MonoBehaviour {

        [SerializeField] private string tagObjectCanCollideWith;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private float[] movementsTimes;

        private Transform oldParent;

        private Vector3 nextPosition;
        private Vector3 startPosition;

        private int index = 1;
        private float elapsedTime = 0;

        private void Start()
        {
            startPosition = waypoints[0].position;
            transform.position = startPosition;
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;

            nextPosition = waypoints[index].position;
            transform.position = Vector3.Lerp(startPosition, nextPosition, elapsedTime / movementsTimes[index]);

            if (transform.position == nextPosition)
            {
                startPosition = nextPosition;
                elapsedTime = 0;

                index ++;

                if (index < 0 || index >= waypoints.Length)
                    index = 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D objectHit)
        {
            if (objectHit.CompareTag(tagObjectCanCollideWith))
            {
                oldParent = objectHit.transform.parent;
                objectHit.transform.SetParent(transform);
            }
        }

        private void OnTriggerExit2D(Collider2D objectUnHit)
        {
            objectUnHit.transform.SetParent(oldParent);
        }

    }
}