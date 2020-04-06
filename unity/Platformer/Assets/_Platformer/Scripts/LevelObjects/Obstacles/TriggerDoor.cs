///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 03/02/2020 20:45
///-----------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System;

namespace Com.Isartdigital.Platformer.LevelObjects.Obstacles {
    public class TriggerDoor : MonoBehaviour {

        [SerializeField] private Transform pivot;
        [SerializeField] private Transform pivotContainer;
        [Space]
        [SerializeField] private Lever lever;
        [Space]
        [SerializeField] private float angle;
        [SerializeField] private float duration;
        [SerializeField] private AnimationCurve speedCurve;

        private Quaternion startRotation;

        public event Action<TriggerDoor, Lever> OnTrigger;

        private void Start() {
            pivot.parent = transform.parent;
            transform.SetParent(pivotContainer);
            startRotation = pivotContainer.rotation;

            lever.OnTriggerTag += Lever_OnTriggerTag;
        }

        private void Lever_OnTriggerTag(Lever sender) {
            OnTrigger?.Invoke(this, sender);
            
            StartCoroutine(OpenCoroutine());

        }

        public void ResetPosition() {
            pivotContainer.rotation = startRotation;
        }

        private IEnumerator OpenCoroutine() {
            float elapsedTime = 0;
            float coef;

            Quaternion endRotation = Quaternion.AngleAxis(angle, Vector3.forward) * startRotation;

            GetComponent<AudioSource>().Play();

            while (elapsedTime < duration) {
                elapsedTime += Time.deltaTime;
                coef = speedCurve.Evaluate(elapsedTime / duration);

                pivotContainer.rotation = Quaternion.Slerp(startRotation, endRotation, coef);

                yield return null;
            }
        }
    }
}