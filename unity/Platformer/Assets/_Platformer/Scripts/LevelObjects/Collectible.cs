///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 21/01/2020 12:07
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.Managers;
using Com.IsartDigital.PlatFormer.Platformer.Utils;
using Com.MaximilienGalea.Test;
using System;
using System.Collections;
using UnityEngine;

namespace Com.Isartdigital.Platformer.LevelObjects {
	public class Collectible : MonoBehaviour {

        public event Action<Collectible,float> OnCollected;

        [SerializeField] private Transform collectibleExplosionPrefab;
        [SerializeField] private string playerTag;
        [SerializeField] private GameObject gfx;
        [SerializeField] private float timeEarn;
        [SerializeField] private Animator animator;


        [SerializeField] private GameObject curvePrefab;
        private CurveLine curveLine;

        private void Start()
        {
            animator.Play("Collectible_Animation", 0, UnityEngine.Random.value);
        }

        private void OnTriggerEnter2D(Collider2D objectHit)
        {

            if (objectHit.CompareTag(playerTag))
            {
                OnCollected?.Invoke(this,timeEarn);

                Transform explosion = Instantiate(collectibleExplosionPrefab, transform.position, transform.rotation);
                curveLine = Instantiate(curvePrefab, transform.position, transform.rotation,transform).GetComponent<CurveLine>();

                explosion.GetComponent<FollowCurve>().setCurve(curveLine);

                GetComponent<Collider2D>().enabled = false;

                gfx.SetActive(false);
                Destroy(explosion.gameObject, 5);
                Destroy(gameObject, 10);
            }
        }

    }
}