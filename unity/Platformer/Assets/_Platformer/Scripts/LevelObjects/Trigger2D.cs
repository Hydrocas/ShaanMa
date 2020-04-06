///-----------------------------------------------------------------
/// Author : Loïc Jacob
/// Date : 29/01/2020 17:31
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.CustomEvent;
using UnityEngine;
using UnityEngine.Events;

namespace Com.Isartdigital.Platformer.LevelObjects {

    [RequireComponent(typeof(Collider2D))]

    public class Trigger2D : MonoBehaviour {

        [SerializeField] protected TransformUnityEvent _onEnter;
        [SerializeField] protected TransformUnityEvent _onExit;

        virtual public event UnityAction<Transform> OnEnter {
            add { _onEnter.AddListener(value); }
            remove { _onEnter.RemoveListener(value); }
        }

        virtual public event UnityAction<Transform> OnExit {
            add { _onExit.AddListener(value); }
            remove { _onExit.RemoveListener(value); }
        }

        virtual protected void OnTriggerEnter2D(Collider2D collision) {
            
            _onEnter.Invoke(collision.transform);
        }

        virtual protected void OnTriggerExit2D(Collider2D collision) {
            _onExit.Invoke(collision.transform);
        }
    }
}