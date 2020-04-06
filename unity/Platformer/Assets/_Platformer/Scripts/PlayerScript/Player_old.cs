/////-----------------------------------------------------------------
///// Author : Maximilien Galea
///// Date : 18/01/2020 17:08
/////-----------------------------------------------------------------

//using Com.IsartDigital.Common.Objects;
//using System;
//using System.Collections;
//using UnityEngine;

//namespace Com.Isartdigital.Platformer.PlayerScript {
//    [RequireComponent(typeof(Rigidbody2D))]
//    public class Player_old : StateObject {

//        [SerializeField] private PlayerController controller;
//        [SerializeField] private PlayerSettings settings;
//        [SerializeField] private PlayerParticleSettings particle;
//        [Space]
//        [SerializeField] private Transform _feetPos;
//        [SerializeField] private ParticleSystem _run;

//        public event Action<Player_old> OnDie;

//        private RaycastHit2D _isGrounded;

//        private bool jumpRequest = false;
//        private bool holdingJump = false;
//        private int facingDirection = 1; // 1 = right, -1 = left
//        private bool isFacingRight = true;

//        public bool getIsFacingRight()
//        {
//             return isFacingRight; 
//        }

//        private Vector2 _groundTilt;

//        private float groundSpeed;

//        private float movementAccDirection;
//        private float inputDirection;
//        private float prevInputDirection;
//        private float lastHorizontalAxis;
//        private float decelerationDirection;

//        private float elapsedTimeAcc;

//        public RaycastHit2D IsGrounded {
//            get { return _isGrounded; }
//            protected set {
//                if (value != _isGrounded) {
//                    if (value) { // land
//                        Instantiate(particle.Landing, _feetPos, false);
//                        _airJump = settings.AirJumpCount;
//                    } else { // jump
//                        Instantiate(particle.Landing, _feetPos, false);
//                    }
//                }
//                _isGrounded = value;
//                rigidbody.isKinematic = value;
//            }
//        }

//        private bool _isWalled;
//        public bool IsWalled {
//            get { return _isWalled; }
//            protected set {
//                if (value != _isWalled) {
//                    _isWalled = value;
//                    if (value) { // land on wall
//                        Instantiate(particle.WallSliding, transform, false);
//                    } else { // exit wall
//                        Instantiate(particle.Landing, transform, false);
//                    }
//                }
//                rigidbody.isKinematic = false;
//            }
//        }

//        public void Respawn(Vector2 spawnPos) {
//            rigidbody.velocity = Vector2.zero;
//            rigidbody.position = spawnPos;
//        }

//        private bool _isWalledRight = false;
//        private bool _isWalledLeft = false;

//        public Vector2 GroundTilt {
//            get { return _groundTilt.normalized; }
//            protected set {
//                _groundTilt = value;
//            }
//        }

//        private new Rigidbody2D rigidbody;
//        private uint _airJump; // nombre de saut en l'air restant;
//        private float elapsedTimeDec;

//        private void Awake() {
//            rigidbody = GetComponent<Rigidbody2D>();
//            Init();
//        }

//        private void FixedUpdate() {
//            SnapPlayerOnGround();
//            CheckSurroundings();
//            CheckMovementDirection();

//            DoAction();
//        }
//        private void Update() {
//            CheckInput();
//            DrawDebug();
//        }

//        private void CheckInput() {
//            inputDirection = controller.HorizontalRawAxis;

//            if (inputDirection != 0 && prevInputDirection != inputDirection) {
//                prevInputDirection = inputDirection;
//            }

//            //Debug.Log(inputDirection); // is ok

//            if (controller.Jump) {
//                jumpRequest = true;
//            } else if (controller.HoldJump) {
//                holdingJump = true;
//            } else {
//                holdingJump = false;
//            }
//        }

//        // on ground
//        private void SetModeMove() {
//            rigidbody.gravityScale = settings.Gravity;
//            DoAction = DoActionMove;
//        }

//        // on walled
//        private void SetModeWallSliding() {
//            DoAction = DoActionWallSliding;
//        }

//        // on air
//        private void SetModeAirborn() {
//            DoAction = DoActionAirborn;
//            decelerationDirection = Mathf.Clamp(rigidbody.velocity.x, -1, 1);
//        }

//        private void DoActionMove() {
//            // acc
//            if (inputDirection != 0 && elapsedTimeAcc < settings.TimeToAcceleration) {
//                elapsedTimeAcc += Time.fixedDeltaTime;
//                //elapsedTimeDec = 0;

//                movementAccDirection = settings.AccelerationCurve.Evaluate(elapsedTimeAcc / settings.TimeToAcceleration);

//                movementAccDirection *= inputDirection;

//                elapsedTimeDec = 1 - (Mathf.Abs(rigidbody.velocity.x) * GroundTilt.x) / settings.HorizontalSpeed;

//                //decelerationDirection = movementAccDirection;

//                // dec
//            } else if (inputDirection == 0 && elapsedTimeDec < settings.TimeToDeceleration) {
//                elapsedTimeDec += Time.fixedDeltaTime;

//                elapsedTimeAcc = 0;

//                //Debug.Log((rigidbody.velocity.x * GroundTilt.x) / settings.HorizontalSpeed);

//                movementAccDirection = settings.DecelerationCurve.Evaluate(elapsedTimeDec / settings.TimeToDeceleration);
//                movementAccDirection *= Mathf.Clamp(rigidbody.velocity.x, -1, 1);
//                //Debug.Log(movementAccDirection);
//            }

//            Vector2 velocity = GroundTilt * movementAccDirection * settings.HorizontalSpeed;
//            velocity = Vector2.ClampMagnitude(velocity, settings.HorizontalSpeed);
//            rigidbody.velocity = velocity;



//            if (jumpRequest) Jump();
//        }

//        private void DoActionWallSliding() {
//            if (rigidbody.velocity.y < -settings.WallSlideSpeedDown && rigidbody.velocity.y < 0) {
//                rigidbody.velocity = new Vector2(rigidbody.velocity.x, -settings.WallSlideSpeedDown);
//            }

//            Vector2 getOffWallForce = new Vector2(settings.WallSlideGetOffForce, 0) * facingDirection;

//            if (_isWalledLeft && inputDirection > 0) {
//                rigidbody.AddForce(getOffWallForce, ForceMode2D.Impulse);
//            } else if (_isWalledRight && inputDirection < 0) {
//                rigidbody.AddForce(getOffWallForce, ForceMode2D.Impulse);
//            }

//            if (jumpRequest) WallJump();
//        }

//        private void DoActionAirborn() {
//            if (jumpRequest) AirJump();

//            if (holdingJump && rigidbody.velocity.y > 0) {
//                rigidbody.gravityScale = settings.Gravity;
//            } else {
//                rigidbody.gravityScale = settings.Gravity;
//            }

//            AirMove();
//        }

//        private void CheckSurroundings() {
//            // ground check
//            Vector2 origin = rigidbody.position + Vector2.up * settings.GroundedRaycastDistance;
//            IsGrounded = Physics2D.Raycast(_feetPos.position, Vector2.down, settings.GroundedRaycastDistance + settings.JumpTolerance, settings.GroundMask);

//            Vector2 groundNormal = IsGrounded.normal;
//            _groundTilt = new Vector2(groundNormal.y, -groundNormal.x);

//            if (_isGrounded) {
//                if (DoAction != DoActionMove) SetModeMove();
//            } else {
//                SetModeAirborn();
//            }
//            // wall check
//            Vector2 wallJumpOrigin = rigidbody.position;
//            Vector2 direction = new Vector2(rigidbody.velocity.x, 0).normalized;
//            RaycastHit2D right = Physics2D.Raycast(wallJumpOrigin, Vector2.right + new Vector2(0, 0.5f), settings.WalledRaycastDistance + settings.WallJumpTolerance, settings.GroundMask);
//            RaycastHit2D left = Physics2D.Raycast(wallJumpOrigin, Vector2.left + new Vector2(0, 0.5f), settings.WalledRaycastDistance + settings.WallJumpTolerance, settings.GroundMask);

//            _isWalledRight = right;
//            _isWalledLeft = left;
//            IsWalled = left || right;

//            if (_isWalled && !_isGrounded) SetModeWallSliding();

//        }

//        private void SnapPlayerOnGround() {
//            if (IsGrounded) {
//                rigidbody.transform.position = IsGrounded.point;

//            }
//        }

//        private void DrawDebug() {
//            //Debug.DrawRay(rigidbody.position, Vector3.right * movementDirection * 10, Color.red);
//            Debug.DrawLine(rigidbody.position + new Vector2(0, 0.5f), rigidbody.position + Vector2.right + new Vector2(0, 0.5f) * (settings.WalledRaycastDistance + settings.WallJumpTolerance), Color.cyan, 0.5f);
//            Debug.DrawLine(rigidbody.position + new Vector2(0, 0.5f), rigidbody.position + Vector2.left + new Vector2(0, 0.5f) * (settings.WalledRaycastDistance + settings.WallJumpTolerance), Color.cyan, 0.5f);
//            //Debug.DrawLine(_feetPos.position, new Vector2(_feetPos.position.x, _feetPos.position.y) + Vector2.down * (settings.GroundedRaycastDistance + settings.JumpTolerance), Color.blue, .5f);
//            Debug.DrawLine(rigidbody.position, rigidbody.position + rigidbody.velocity, Color.magenta, .5f);
//            //Debug.DrawLine(rigidbody.transform.position, (Vector2)rigidbody.transform.position + GroundNormal * 10, Color.green);
//            //Debug.DrawLine(rigidbody.transform.position, (Vector2)rigidbody.transform.position + _groundTilt * 100, Color.yellow);
//        }

//        private void AirMove() {
//            rigidbody.AddForce(new Vector2(inputDirection * settings.HorizontalAirSpeed, 0));

//            if (rigidbody.velocity.y > 0) {
//                rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, settings.MaxAirSpeed);
//            }
//        }

//        private void WallJump() {
//            jumpRequest = false;

//            Vector2 forceToAdd = settings.NormalizedWallJumpDirection * settings.WallJumpForce;
//            forceToAdd.x *= -facingDirection;

//            rigidbody.velocity = forceToAdd;

//            Flip();

//            Debug.DrawLine(rigidbody.position, rigidbody.position + forceToAdd, Color.red, 2);
//        }

//        private void Jump() {
//            rigidbody.isKinematic = false;
//            jumpRequest = false;
//            //if (moveCoroutine != null) StopCoroutine(moveCoroutine);
//            rigidbody.AddForce(new Vector2(0, settings.JumpMaxHight), ForceMode2D.Impulse);
//        }

//        private void AirJump() {
//            jumpRequest = false;
//            if (_airJump <= 0) {
//                return;
//            }

//            _airJump--;

//            if (inputDirection != 0) {
//                rigidbody.velocity = new Vector2(Mathf.Abs(rigidbody.velocity.magnitude) * inputDirection, settings.AirJumpHight);
//            } else {
//                rigidbody.velocity = new Vector2(rigidbody.velocity.x, settings.AirJumpHight);
//            }
//        }

//        private void CheckMovementDirection() {
//            if (isFacingRight && inputDirection < 0) {
//                Flip();
//            } else if (!isFacingRight && inputDirection > 0) {
//                Flip();
//            }
//        }

//        private void Flip() {
//            isFacingRight = !isFacingRight;
//            facingDirection *= -1;
//        }

//        public void Die() {
//            Debug.Log("<size=21>je suis mort</size>");
//            OnDie?.Invoke(this);
//        }
//    }
//}