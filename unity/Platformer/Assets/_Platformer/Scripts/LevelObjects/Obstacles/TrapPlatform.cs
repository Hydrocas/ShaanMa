///-----------------------------------------------------------------
/// Author : Loïc Jacob
/// Date : 27/01/2020 15:45
///-----------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects.Obstacles {
	public class TrapPlatform : MonoBehaviour {

        [SerializeField] private string tagObjectCanCollideWith;

        [SerializeField] private Transform killzone;
        [SerializeField] private Transform killzoneStartTransform;
        [SerializeField] private Transform killzoneEndTransform;

        [SerializeField] private float secondsBeforeTrapOut;
        [SerializeField] private float secondsBeforeTrapIn;
        [SerializeField] private float trapOutDuration;
        [SerializeField] private float trapInDuration;

        private Vector3 killzoneStartPosition;
        private Vector3 killzoneEndPosition;
        private float elapsedTime;

        private void OnTriggerEnter2D(Collider2D objectHit)
        {
            if (objectHit.CompareTag(tagObjectCanCollideWith))
                StartCoroutine(DelayToMove(secondsBeforeTrapOut, killzoneStartTransform.position, killzoneEndTransform.position, true));
        }

        //Coroutine appellée deux fois, une fois quand on marche sur la plateform alors que la killzone est rentrée et une fois quand elle est sortie.
        private IEnumerator DelayToMove(float seconds, Vector3 startPoint, Vector3 endPoint, bool isGoingOut)
        {
            yield return new WaitForSeconds(seconds);

            killzoneStartPosition   = startPoint;
            killzoneEndPosition     = endPoint;

            if (isGoingOut)
                StartCoroutine(MoveTo(trapOutDuration, true));
            else
                StartCoroutine(MoveTo(trapInDuration, false));
        }

        //Coroutine appellée deux fois, une fois quand le délais avant de faire sortir la killzone est terminé et une fois quand le délais avant de rentrer la killzone est terminé.
        private IEnumerator MoveTo(float trapMovementDuration, bool isGoingIn)
        {
            elapsedTime = 0;

            while (killzone.position != killzoneEndPosition)
            {

                elapsedTime += Time.deltaTime;

                killzone.position = Vector3.Lerp(killzoneStartPosition, killzoneEndPosition, elapsedTime / trapMovementDuration);
                yield return null;
            }

            if (isGoingIn)
                StartCoroutine(DelayToMove(secondsBeforeTrapIn, killzoneEndTransform.position, killzoneStartTransform.position, false));
            
        }

    }
}