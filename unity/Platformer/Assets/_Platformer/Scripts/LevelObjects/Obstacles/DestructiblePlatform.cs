///-----------------------------------------------------------------
/// Author : Loïc Jacob
/// Date : 27/01/2020 11:13
///-----------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects.Obstacles {
    public class DestructiblePlatform : MonoBehaviour
    {

        [SerializeField] private string tagObjectCanCollideWith;

        [SerializeField] private GameObject gfx;
        [SerializeField] private GameObject ground;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private float secondsToDestruct;
        [SerializeField] private float secondsToRepare;
        [SerializeField] private float xShakeSpeed;
        [SerializeField] private float xShakeAmount;
        [SerializeField] private float yShakeSpeed;
        [SerializeField] private float yShakeAmount;

        private ParticleSystem _particle;
        private Vector3 startPos;

        private void Start() {
            _particle = particle;
            startPos = transform.position;
        }

        private void OnTriggerEnter2D(Collider2D objectHit)
        {
            if (objectHit.CompareTag(tagObjectCanCollideWith)) {
                StartCoroutine(WaitTo(secondsToDestruct, true));
                StartCoroutine(Shake(secondsToDestruct));
                _particle.Play();
            }

        }

        private float elapsedTime;
        private IEnumerator Shake(float second) {
            while (elapsedTime < second) {
                elapsedTime += Time.fixedDeltaTime;
                transform.position = startPos + new Vector3(Mathf.Sin(elapsedTime * xShakeSpeed) * xShakeAmount, Mathf.Sin(elapsedTime * yShakeSpeed) * yShakeAmount);
                yield return new WaitForFixedUpdate();
            }
            StopCoroutine(Shake(second));
            elapsedTime = 0;
        }

        private IEnumerator WaitTo(float seconds, bool isGoingToBeDestroy)
        {
            yield return new WaitForSeconds(seconds);

            if (isGoingToBeDestroy)
            {
                EnableObject(false);
                GetComponent<AudioSource>().Play();

                StartCoroutine(WaitTo(secondsToRepare, false));
            } else {
                EnableObject(true);
                transform.position = startPos;
            }
                        
        }

        private void EnableObject(bool isDisable)
        {
            gfx.GetComponent<Renderer>().enabled = isDisable;
            ground.GetComponent<Collider2D>().enabled = isDisable;
            gameObject.GetComponent<Collider2D>().enabled = isDisable;
        }
    }
}