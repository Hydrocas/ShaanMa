///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 18/01/2020 17:08
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.PlayerScript {
    [CreateAssetMenu(fileName = "PlayerController", menuName = "Player/PlayerController")]
    public class PlayerController : ScriptableObject {

        [SerializeField] private string _horizontalAxis;
        [SerializeField] private string _verticalAxis;
        [SerializeField] private string _jumpButton;
        [SerializeField] private KeyCode _die = KeyCode.R;
        [SerializeField] private float inputDistance;

        private int jumpFingerId;
        private int moveFingerId;
        private Vector2 startPos;

        public float HorizontalAxis {
            get {
                return Input.GetAxis(_horizontalAxis);
            }
        }

        public float HorizontalRawAxis {
            get {
#if (UNITY_ANDROID)
                for (int i = 0; i < Input.touchCount; i++) {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began) {
                        startPos = touch.position;
                    } else if (startPos.x < Screen.width / 2) {
                        moveFingerId = touch.fingerId;

                        
                        if (touch.phase == TouchPhase.Moved) {
                            float curentDistance = touch.position.x - startPos.x;

                            if (Mathf.Abs(curentDistance) > inputDistance) {
                                startPos.x += (curentDistance) + inputDistance * -Mathf.Sign(curentDistance);
                            }
                        }
                        return Mathf.Sign(touch.position.x - startPos.x);
                        //}
                    }
                }
                return 0;
#else
                return Input.GetAxisRaw(_horizontalAxis);
#endif
            }
        }

        public float VerticalAxis {
            get {
                return Input.GetAxis(_verticalAxis);
            }
        }

        public bool Jump {
            get {
#if (UNITY_ANDROID)
                for (int i = Input.touchCount - 1; i >= 0; i--) {

                    Touch touch = Input.GetTouch(i);
                    if (touch.position.x > Screen.width / 2) {
                        jumpFingerId = touch.fingerId;
                        TouchPhase phase = touch.phase;
                        return phase == TouchPhase.Began;
                    }
                }
                return false;
#else
                return Input.GetButtonDown(_jumpButton);

#endif
            }
        }

        public bool HoldJump {
            get {
#if (UNITY_ANDROID)
                Touch touch = Input.GetTouch(jumpFingerId);
                return touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved;
#else
                return Input.GetButton(_jumpButton);
#endif
            }
        }

        public bool Die {
            get {
                return Input.GetKeyDown(_die);
            }
        }
    }
}