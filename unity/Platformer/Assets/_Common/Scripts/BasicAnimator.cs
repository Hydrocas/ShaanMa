///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 28/11/2019 17:25
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Interface;
using UnityEngine;

namespace Com.IsartDigital.Common {
    [RequireComponent(typeof(Animator))]
	public class BasicAnimator : MonoBehaviour, IBasicAnimator {

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        virtual public void Appear()
        {
            gameObject.SetActive(true);
            animator.SetTrigger(BasicAnimatorState.Appear.ToString());
        }

        virtual public void Disappear()
        {
            animator.SetTrigger(BasicAnimatorState.Disappear.ToString());
        }

        virtual public void EndAppear()
        {
            
        }

        virtual public void EndDisappear()
        {
            gameObject.SetActive(false);
        }
	}
}