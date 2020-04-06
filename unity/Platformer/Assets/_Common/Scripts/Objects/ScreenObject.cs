///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 10/11/2019 15:23
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Interface;
using Com.IsartDigital.Common.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Com.IsartDigital.Common.Objects {
    public delegate void ScreenObjectEventHandler(ScreenObject sender);

    [RequireComponent(typeof(Animator)), RequireComponent(typeof(CanvasGroup))]
	public class ScreenObject : GameplayObject, IBasicAnimator {

        private Animator animator;
        private CanvasGroup canvasGroup;
        private Canvas canvas;
        private Button[] buttons;
        protected ScreensDisplayer screenDisplayer;
        private Coroutine appearedCoroutine;

        public bool Interectable
        {
            set => canvasGroup.interactable = value;
            get => canvasGroup.interactable;
        }

        #region Events

        public event ScreenObjectEventHandler OnAppearEnd;
        public event ScreenObjectEventHandler OnDisappearEnd;

        [Space]
        [SerializeField] private UnityEvent onAppear;
        [SerializeField] private UnityEvent onDisappear;

        public event UnityAction OnApppear
        {
            add { onAppear.AddListener(value); }
            remove { onAppear.RemoveListener(value); }
        }

        public event UnityAction OnDisappear
        {
            add { onDisappear.AddListener(value); }
            remove { onDisappear.RemoveListener(value); }
        }

        #endregion

        public void Init(ScreensDisplayer screenDisplayer)
        {
            animator = GetComponent<Animator>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponent<Canvas>();
            buttons = GetComponentsInChildren<Button>();
            this.screenDisplayer = screenDisplayer;

            canvasGroup.interactable = false;
            canvas.enabled = false;
        }

        #region Appear

        public virtual void Appear()
        {
            canvas.enabled = true;
            animator.SetTrigger(BasicAnimatorState.Appear.ToString());
            onAppear?.Invoke();
        }

        public virtual void ForceAppear()
        {
            canvas.enabled = true;
            animator.Play(BasicAnimatorState.Appear.ToString(), 0, 1);
            onAppear?.Invoke();
        }

        public virtual void EndAppear()
        {
            canvasGroup.interactable = true;
            OnAppearEnd?.Invoke(this);

            appearedCoroutine = StartCoroutine(AppearedRoutine());
        }

        #endregion

        #region Disappear

        public virtual void Disappear()
        {
            canvasGroup.interactable = false;
            animator.SetTrigger(BasicAnimatorState.Disappear.ToString());

            for (int i = buttons.Length - 1; i >= 0; i--)
            {
                buttons[i].onClick.RemoveAllListeners();
            }

            onDisappear?.Invoke();
        }

        public virtual void ForceDisappear()
        {
            canvasGroup.interactable = false;
            animator.Play(BasicAnimatorState.Disappear.ToString(), 0, 1);

            for (int i = buttons.Length - 1; i >= 0; i--)
            {
                buttons[i].onClick.RemoveAllListeners();
            }

            onDisappear?.Invoke();
        }

        public virtual void EndDisappear()
        {
            canvas.enabled = false;
            OnDisappearEnd?.Invoke(this);

            if (appearedCoroutine == null) return;

            StopCoroutine(appearedCoroutine);
            appearedCoroutine = null;
        }

        #endregion

        #region Loop

        private IEnumerator AppearedRoutine()
        {
            while (true)
            {
                AppearedUpdate();
                yield return null;
            }
        }

        protected virtual void AppearedUpdate()
        {

        }

        #endregion
    }
}