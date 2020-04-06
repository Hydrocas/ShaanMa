

using Cinemachine;
using Com.Isartdigital.Common.Interface;
using System;
using UnityEngine;
///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 21/01/2020 11:45
///-----------------------------------------------------------------
namespace Com.Isartdigital.Platformer.LevelObjects {
    public class Checkpoint : Trigger2D {

        public event Action<Checkpoint> onTrigger;

        [SerializeField] private RipplePostProcessor cam;
        [SerializeField] private float rippleMaxAmout;
        [SerializeField, Range(0f, 1f)] private float rippleFriction;
        [SerializeField] Collider2D bellCollider;

        private bool isTriggred;
        override protected void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (isTriggred)
                return;
          

            base.OnTriggerEnter2D(collision);
            isTriggred = true;
            bellCollider.enabled = false;

            onTrigger?.Invoke(this);

            cam.RippleEffect(transform, rippleMaxAmout, rippleFriction);

            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        }
    }
}