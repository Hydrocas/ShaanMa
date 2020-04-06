///-----------------------------------------------------------------
/// Author : Loïc Jacob
/// Date : 11/02/2020 15:27
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.CustomEvent;
using UnityEngine;
using UnityEngine.Events;

namespace Com.Isartdigital.Platformer.LevelObjects {
	public class DirectionalTrigger : MonoBehaviour {

        [SerializeField] protected TransformUnityEvent _onEnterLeft;
        [SerializeField] protected TransformUnityEvent _onEnterRight;
        [SerializeField] protected TransformUnityEvent _onExit;

        virtual public event UnityAction<Transform> OnEnterLeft
        {
            add { _onEnterLeft.AddListener(value); }
            remove { _onEnterLeft.RemoveListener(value); }
        }

        virtual public event UnityAction<Transform> OnEnterRight
        {
            add { _onEnterRight.AddListener(value); }
            remove { _onEnterRight.RemoveListener(value); }
        }

        virtual public event UnityAction<Transform> OnExit
        {
            add { _onExit.AddListener(value); }
            remove { _onExit.RemoveListener(value); }
        }

        virtual protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.position.x > GetComponent<Collider2D>().bounds.center.x)
                _onEnterRight.Invoke(collision.transform);
            else
                _onEnterLeft.Invoke(collision.transform);
                
        }

        virtual protected void OnTriggerExit2D(Collider2D collision)
        {
            _onExit.Invoke(collision.transform);
        }
    }
}