///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 27/01/2020 16:38
///-----------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Com.Isartdigital.Common.Audio.Emitter;
using Com.IsartDigital.Common.Objects;
using UnityEngine;

namespace Com.Isartdigital.Platformer.PlayerScript.Physics {
    public class PlayerPhysique : StateObject {

        [SerializeField] private PlayerController controller;
        [SerializeField] private PlayerSettings settings;
        [SerializeField] private GameObject gfxContainer;
        [SerializeField] private GameObject DropMask;
        [SerializeField] private Transform _feetPos;
        [SerializeField] private Animator _animator;

        //Cinemachine Virtual Camera
        [SerializeField] private CameraTarget cameraTarget;
        [SerializeField] private CinemachineVirtualCamera vCam;
        private CinemachineFramingTransposer vCamBody;
        private float lastLookAheadTime;
        private float lastLookAheadSmoothing;
        //

        [Space]
        [SerializeField] private AleaAudioSource footStep;
        [SerializeField] private AudioSource jump;
        [SerializeField] private AudioSource doubleJump;
        [SerializeField] private AudioSource slide;
        [SerializeField] private AudioSource wallJump;

        public event Action<PlayerPhysique> OnDie;
        public event Action<int> eventParticles;

        private PlayerRaycast playerRaycast;
        public bool isGrounded { get; protected set; }
        public bool isSlidingWall { get; protected set; }

        private Vector2 _velocity;
        private bool isDead;

        public Vector2 getVelocity() {
            return _velocity;
        }

        private uint _airJumpCount;

        public Vector3 GroundTilt {
            get 
            { 
                return _groundTilt.normalized; 
            }
            protected set 
            {
                _groundTilt = value;
            }
        }

        private float inputDirection;

        private Vector3 _groundTilt;
        public bool _isWalledRight { get; protected set; }
        public bool _isWalledLeft { get; protected set; }
        private bool isFacingRight;
        public int FacingDirection { get; protected set; }

        private new Rigidbody2D rigidbody;
        private float _velocityRatio;
        private float horizontalSpeed;
        private float elapsedTime;

        private Vector2 startPos;
        private Vector2 endPos;

        private bool holdingJump;
        private bool jumpRequest;

        private Coroutine coyoteCoroutine;

        private JumpConfig currentJumpConfig;
        private int jumpDirection;

        private Vector2 _moveDirection;
        private bool isCoyoteWall;
        private int lastWallDirection;

        public Vector2 MoveDirection {
            get {
                if (inputDirection != 0) _moveDirection = _groundTilt * inputDirection;
                return _moveDirection;
            }
        }

        private int WallDirection => _isWalledRight? 1: -1;

        private bool HeadCollision
        {
            get
            {
                RaycastHit2D hit = playerRaycast.HeadRaycasts;
                return hit && !hit.collider.CompareTag("Traversable");
            }
        }

        private bool IsCloseToGround
        {
            get 
            { 
                return playerRaycast.AirGroundRaycastsSnap;
            }
        }

        private void Awake()
        {
            vCamBody = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            playerRaycast = GetComponent<PlayerRaycast>();
            rigidbody = GetComponent<Rigidbody2D>();
            FacingDirection = -1;
            _airJumpCount = settings.AirJumpCount;
        }

        private void Start()
        {
            Init();
            Flip();
        }

        public override void Init() 
        {
            SetModeFall();
        }

        private void FixedUpdate() 
        {
            DoAction();
            AnimUpdate();
        }

        private void Update() 
        {
            CheckInput();
            DrawDebug();
        }

        private void AnimUpdate() 
        {
            _animator.SetBool("IsInAir", !isGrounded);
            _animator.SetFloat("VerticalSpeed", _velocity.y);
            _animator.SetFloat("HorizontalSpeed", Mathf.Abs(_velocity.x));
            _animator.SetBool("IsWalled", isSlidingWall);
        }

        #region MoveGround

        private void SetModeMoveGround() 
        {
            isSlidingWall = false;
            DoAction = DoActionMoveGround;
            horizontalSpeed = 0;

            footStep.PlayContinues();

            if (inputDirection != 0)
                elapsedTime = settings.TimeToAcceleration;
        }

        private void DoActionMoveGround() 
        {
            isGrounded = CheckGround();

            MoveGround();
            CheckMovementDirection();

            if (jumpRequest) 
            {
                StopCoyoteRoutine();
                SetModeJump();
            } 
            else if (!isGrounded) 
            {
                CoyoteTime();
            }
        }

        private void MoveGround() 
        {
            if (inputDirection != 0) 
            {
                if (!footStep.IsPlaying) footStep.PlayContinues();

                elapsedTime += Time.fixedDeltaTime;

                _velocityRatio = settings.AccelerationCurve.Evaluate(elapsedTime / settings.TimeToAcceleration);
                horizontalSpeed = Mathf.Lerp(0, settings.HorizontalMaxSpeed, _velocityRatio);
            }
            if (inputDirection == 0 || Mathf.Sign(_velocity.x) != Mathf.Sign(inputDirection)) 
            {
                footStep.Stop();

                elapsedTime = 0f;
                horizontalSpeed *= Mathf.Pow(settings.GroundFriction, Time.fixedDeltaTime);
            }

            horizontalSpeed = Mathf.Abs(horizontalSpeed);
            _velocity = MoveDirection * horizontalSpeed;
            
            rigidbody.MovePosition(rigidbody.position + _velocity * Time.fixedDeltaTime);
        }

        private bool CheckGround() 
        {
            RaycastHit2D hit2D = playerRaycast.GroundRaycasts;

            if (hit2D && inputDirection != 0) {
                transform.position = new Vector3(transform.position.x, hit2D.point.y);
            }

            if (hit2D.normal.y > Mathf.Sqrt(2) / 2) {
                Vector3 groundNormal = hit2D.normal;
                _groundTilt = new Vector3(groundNormal.y, -groundNormal.x);

            }

            return hit2D;
        }

        #endregion

        #region Jump

        private void SetModeJump() {
            eventParticles(3);

            isGrounded = false;
            jumpRequest = false;

            InitJump(settings.JumpConfig);
            footStep.Stop();
            jump.Play();

            DoAction = DoActionJump;
        }

        private void DoActionJump() 
        {
            AirMove();
            Jump();
            CheckMovementDirection();

            if (HeadCollision || CompareJumpHight() || (!holdingJump && CompareJumpHight(startPos.y + settings.JumpMinHight))) 
            {
                _velocity.y = 0;
                SetModeFall();
            }
            else if (CheckWalls(inputDirection)) 
            {
                SetModeWallSlide();
            }
            if (jumpRequest)
            {
                SetModeAirJump();
            }
        }

        private void InitJump(JumpConfig jumpConfig)
        {
            currentJumpConfig = jumpConfig;

            startPos = rigidbody.position;
            endPos = currentJumpConfig.CalcEndPos(startPos);

            elapsedTime = 0f;
        }

        private void Jump() 
        {
            elapsedTime += Time.fixedDeltaTime;

            Vector2 nextPos = rigidbody.position;

            float coef = currentJumpConfig.verticalCurve.Evaluate(elapsedTime / currentJumpConfig.jumpTime);
            nextPos.y = Mathf.Lerp(startPos.y, endPos.y, coef);

            if(currentJumpConfig.horizontalDistance != 0)
            {
                coef = currentJumpConfig.horizontalCurve.Evaluate(elapsedTime / currentJumpConfig.jumpTime);
                nextPos.x = Mathf.Lerp(startPos.x, endPos.x, coef);
            }

            nextPos.x += _velocity.x * Time.fixedDeltaTime;

            _velocity.y = (nextPos.y - rigidbody.position.y) / Time.fixedDeltaTime;

            rigidbody.MovePosition(nextPos);
        }

        private bool CompareJumpHight(float endPosY = float.NaN)
        {
            if (endPosY == float.NaN) endPosY = endPos.y;

            float currentPosY = rigidbody.position.y;

            return currentPosY > endPos.y || Mathf.Approximately(currentPosY, endPos.y);
        }

        #endregion

        #region Fall

        private void SetModeFall() 
        {
            isGrounded = false;
            isSlidingWall = false;

            footStep.Stop();

            DoAction = DoActionFall;
        }

        private void DoActionFall() 
        {
            AirMove();
            Fall();
            CheckMovementDirection();

            if (_velocity.y < 0 && CheckAirGround()) 
            {
                SetModeMoveGround();
                StopCoyoteRoutine();
                eventParticles(5);
            }
            else if (jumpRequest) 
            {
                if (CheckWallsSnap())
                {
                    SetModeWallJump();
                }
                else if (isCoyoteWall)
                {
                    SetModeWallJump(-lastWallDirection);
                }
                else if (_airJumpCount > 0 && !IsCloseToGround)
                {
                    SetModeAirJump();
                }

                StopCoyoteRoutine();
            } 
            else if (CheckWalls(inputDirection)) 
            {
                SetModeWallSlide();
                StopCoyoteRoutine();
            }
        }

        private void Fall() 
        {
            _velocity.y += settings.Gravity * Time.fixedDeltaTime;
            rigidbody.MovePosition(rigidbody.position + _velocity * Time.fixedDeltaTime);
        }

        private void AirMove() 
        {
            _velocity.x += settings.AircontrolAccPower * inputDirection * Time.fixedDeltaTime;
            _velocity.x *= Mathf.Pow(settings.AirFriction, Time.fixedDeltaTime);
            _velocity.x = Mathf.Clamp(_velocity.x, -settings.AircontrolMaxSpeed, settings.AircontrolMaxSpeed);
        }

        private bool CheckAirGround() 
        {
            RaycastHit2D hit2D = playerRaycast.AirGroundRaycasts;

            if (!hit2D) return false;

            _airJumpCount = settings.AirJumpCount;
            transform.position = new Vector3(transform.position.x, hit2D.point.y);

            return true;
        }

        #endregion

        #region AirJump

        private void SetModeAirJump() 
        {
            eventParticles(4);
            jumpRequest = false;

            DoAction = DoActionAirJump;

            _airJumpCount--;

            _velocity = new Vector2(settings.AirJumpPower * inputDirection, 0);

            InitJump(settings.AirJumpConfig);

            doubleJump.Play();
            _animator.SetTrigger("DoubleJump");

            isGrounded = false;
        }

        private void DoActionAirJump() 
        {
            AirMove();
            Jump();
            CheckMovementDirection();

            if (CompareJumpHight() || HeadCollision) 
            {
                _velocity.y = 0;
                SetModeFall();
            } 
            else if (jumpRequest) 
            {
                if (_airJumpCount > 0)
                {
                    SetModeAirJump();
                }
            } 
            else if (CheckWalls(inputDirection)) 
            {
                SetModeWallSlide();
            }
        }

        #endregion

        #region WallSlide

        private void SetModeWallSlide() 
        {
            isSlidingWall = true;

            _velocity.y *= settings.WallSlideSpeedRatio;
            _velocity.x = 0;

            slide.Play();

            DoAction = DoActionWallSlide;
        }

        private void DoActionWallSlide() 
        {
            WallMove();

            if (jumpRequest) 
            {
                Flip();
                SetModeWallJump();
                slide.Stop();
            } 
            else if (_velocity.y < 0 && CheckAirGround()) 
            {
                Flip();
                SetModeMoveGround();
                slide.Stop();
            }
            else if((inputDirection == 0 && elapsedTime >= settings.TimeToGetOff) || (inputDirection != 0 && elapsedTime >= settings.TimeToGetOffOppositeDirection))
            {
                _velocity.x += settings.WallSlideGetOffForce * Time.deltaTime * -WallDirection;
                lastWallDirection = WallDirection;

                slide.Stop();
                SetModeFall();
                CoyoteWall();
            }
            else if (!CheckWalls(WallDirection)) 
            {
                slide.Stop();
                SetModeFall();
            }
        }

        private void WallMove() 
        {
            _velocity.y = Mathf.Clamp(_velocity.y - settings.WallSlideSpeedDown,
                -settings.WallSlideMaxSpeedDown, settings.WallSlideMaxSpeedUp);

            if (inputDirection == 0 || (_isWalledLeft && inputDirection > 0) || (_isWalledRight && inputDirection < 0)) 
            {
                elapsedTime += Time.fixedDeltaTime;
            } 
            else
            {
                elapsedTime = 0;
            }

            rigidbody.MovePosition(rigidbody.position + _velocity * Time.fixedDeltaTime);
        }

        #endregion

        #region CheckWalls

        private bool CheckWalls(float direction)
        {
            _isWalledRight = playerRaycast.SideRightRaycasts;
            _isWalledLeft = playerRaycast.SideLeftRaycasts;

            if (direction > 0 && _isWalledRight)
            {
                if (FacingDirection < 0)
                {
                    Flip();
                }

                return !playerRaycast.SideRightRaycasts.collider.CompareTag("Traversable");
            }

            if (direction < 0 && _isWalledLeft)
            {
                if (FacingDirection > 0)
                {
                    Flip();
                }

                return !playerRaycast.SideLeftRaycasts.collider.CompareTag("Traversable");
            }

            return false;
        }

        private bool CheckWalls()
        {
            _isWalledRight = playerRaycast.SideRightRaycasts;
            _isWalledLeft = playerRaycast.SideLeftRaycasts;

            if (_isWalledRight)
            {
                if (FacingDirection > 0)
                {
                    Flip();
                }

                return !playerRaycast.SideRightRaycasts.collider.CompareTag("Traversable");
            }

            if (_isWalledLeft)
            {
                if (FacingDirection < 0)
                {
                    Flip();
                }

                return !playerRaycast.SideLeftRaycasts.collider.CompareTag("Traversable");
            }

            return false;
        }

        private bool CheckWallsSnap()
        {
            _isWalledRight = playerRaycast.SideRightRaycastsSnap;
            _isWalledLeft = playerRaycast.SideLeftRaycastsSnap;

            if (_isWalledRight)
            {
                if (FacingDirection > 0)
                {
                    Flip();
                }

                return !playerRaycast.SideRightRaycastsSnap.collider.CompareTag("Traversable");
            }

            if (_isWalledLeft)
            {

                if (FacingDirection < 0)
                {
                    Flip();
                }

                return !playerRaycast.SideLeftRaycastsSnap.collider.CompareTag("Traversable");
            }

            return false;
        }

        #endregion

        #region WallJump

        private void SetModeWallJump()
        {
            SetModeWallJump(-WallDirection);
        }

        private void SetModeWallJump(int direction)
        {
            DoAction = DoActionWallJump;
            isSlidingWall = false;
            jumpRequest = false;
            jumpDirection = direction;

            JumpConfig jumpConfig = settings.WallJumpConfig;
            jumpConfig.horizontalDistance *= jumpDirection;

            InitJump(jumpConfig);

            eventParticles(8);
            wallJump.Play();
            _animator.SetTrigger("Jump");
        }

        private void DoActionWallJump()
        {
            Jump();

            if (CompareJumpHight() || HeadCollision)
            {
                _velocity.x = settings.WallJumpInertia * jumpDirection;
                _velocity.y = 0;
                SetModeFall();
            }
            else if (CheckWalls(jumpDirection))
            {
                SetModeWallSlide();
            }
            else if (jumpRequest && _airJumpCount > 0 && !CheckWallsSnap())
            {
                SetModeAirJump();
            }
        }

        #endregion

        #region Input

        private void CheckInput() {
            inputDirection = controller.HorizontalRawAxis;

            if (controller.Jump) 
            {
                jumpRequest = true;
            } 
            else if (controller.HoldJump) 
            {
                holdingJump = true;
            } 
            else 
            {
                jumpRequest = holdingJump = false;
            }
        }

        #endregion

        #region Flip

        private void CheckMovementDirection() 
        {
            if (isFacingRight && inputDirection < 0) 
            {
                Flip();
            } 
            else if (!isFacingRight && inputDirection > 0) 
            {
                Flip();
            }
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            FacingDirection *= -1;
            gfxContainer.transform.Rotate(Vector3.up, 180);
        }

        #endregion

        #region Die

        public void Die()
        {
            if (isDead) return;
            isDead = true;
            if (DoAction == DoActionVoid) return;

            gfxContainer.transform.GetChild(1).gameObject.SetActive(false);
            gfxContainer.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);

            dropMask();

            DoAction = DoActionVoid;
            _velocity = Vector2.zero;

            OnDie?.Invoke(this);
        }

        private void dropMask()
        {
            eventParticles(7);
            GameObject droppedMask = Instantiate(DropMask);
            droppedMask.transform.position = transform.position + Vector3.up;
            droppedMask.GetComponent<Rigidbody2D>().AddTorque(UnityEngine.Random.Range(-90, 90));
            droppedMask.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-180, 180), 360));
            Destroy(droppedMask, 120);
        }

        public void Respawn(Vector2 spawnPos)
        {
            DoAction = DoActionFall;

            gfxContainer.transform.GetChild(1).gameObject.SetActive(true);
            gfxContainer.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);

            transform.position = spawnPos;

            StartCoroutine(ReplacePlayer(spawnPos));
            isDead = false;
        }

        private IEnumerator ReplacePlayer(Vector2 position) 
        {
            lastLookAheadTime = vCamBody.m_LookaheadTime;
            lastLookAheadSmoothing = vCamBody.m_LookaheadSmoothing;
            vCamBody.m_LookaheadTime = 0;
            vCamBody.m_LookaheadSmoothing = 0;

            while ((Vector2)transform.position != position) {
                transform.position = position;
                yield return null;
            }

            while (vCamBody.m_LookaheadTime != lastLookAheadTime) {
                vCamBody.m_LookaheadTime = Mathf.MoveTowards(vCamBody.m_LookaheadTime, lastLookAheadTime, 0.1f);
                yield return null;
            }

            vCamBody.m_LookaheadSmoothing = lastLookAheadSmoothing;

            StopCoroutine(ReplacePlayer(position));
        }

        private void DrawDebug() {
            Debug.DrawLine(transform.position + Vector3.up / 2, rigidbody.position + _velocity + Vector2.up / 2, Color.magenta);
        }

        #endregion

        #region CoyoteTime

        private void CoyoteTime()
        {
            if (coyoteCoroutine != null) return;

            coyoteCoroutine = StartCoroutine(WaitAndCallRoutine(SetModeFall, settings.CoyoteTime));
        }

        private void StopCoyoteRoutine()
        {
            if (coyoteCoroutine == null) return;

            StopCoroutine(coyoteCoroutine);
            coyoteCoroutine = null;
            isCoyoteWall = false;
        }

        private IEnumerator WaitAndCallRoutine(Action callback, float duration)
        {
            yield return new WaitForSeconds(duration);

            coyoteCoroutine = null;
            callback();
        }

        private void CoyoteWall()
        {
            if (coyoteCoroutine != null) return;

            isCoyoteWall = true;
            coyoteCoroutine = StartCoroutine(WaitAndCallRoutine(CoyoteWallEnd, settings.WallSlideCoyoteTime));
        }

        private void CoyoteWallEnd()
        {
            isCoyoteWall = false;
        }

        #endregion

        #region Camera
        public void setCameraTarget(Vector3 TargetedPosition)
        {
            cameraTarget.setTargetedPosition(TargetedPosition);
        }

        public void resetCameraTarget(Vector3 TargetedPosition)
        {
            cameraTarget.ResetTargetedPosition(TargetedPosition);
        }

        #endregion
    }
}
 