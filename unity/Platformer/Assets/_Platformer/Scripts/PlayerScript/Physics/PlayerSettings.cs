///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 18/01/2020 17:09
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Platformer.PlayerScript.Physics {
    [CreateAssetMenu(fileName = "New Player settings", menuName = "Player/Settings")]
    public class PlayerSettings : ScriptableObject {

        #region Move

        [Header("Move Param")]

        [SerializeField] private float _horizontalMaxSpeed;
        [SerializeField] private AnimationCurve _accelerationCurve;
        [SerializeField] private float _timeToAcceleration;
        [SerializeField] private float _groundFriction;

        public float HorizontalMaxSpeed => _horizontalMaxSpeed;
        public AnimationCurve AccelerationCurve => _accelerationCurve;
        public float TimeToAcceleration => _timeToAcceleration;
        public float GroundFriction => _groundFriction;

        #endregion

        #region Jump

        [Header("Jump Param")]

        [SerializeField] private float _jumpMinHight;
        [SerializeField] private float _jumpMaxHight;
        [SerializeField] private AnimationCurve _jumpCurve;
        [SerializeField] private float _jumpTime;

        public float JumpMinHight => _jumpMinHight;

        public JumpConfig JumpConfig
        {
            get => new JumpConfig(_jumpTime, _jumpCurve, _jumpMaxHight);
        }

        #endregion

        #region AirJump

        [Header("AirJump Param")]

        [SerializeField] private uint _airJumpCount;
        [SerializeField] private float _airJumpHight;
        [SerializeField] private AnimationCurve _airJumpCurve;
        [SerializeField] private float _airJumpTime;
        [SerializeField] private float _airJumpPower;

        public uint AirJumpCount => _airJumpCount;
        public float AirJumpPower => _airJumpPower;

        public JumpConfig AirJumpConfig
        {
            get => new JumpConfig(_airJumpTime, _airJumpCurve, _airJumpHight);
        }

        #endregion

        #region Fall

        [Header("Fall Param")]

        [SerializeField] private float _gravity;

        [SerializeField] private float _airFriction;
        [SerializeField] private float _aircontrolAccPower;
        [SerializeField] private float _aircontrolMaxSpeed;

        [SerializeField] private float coyoteTime = 1;

        public float Gravity => _gravity;
        public float AirFriction => _airFriction;
        public float AircontrolAccPower => _aircontrolAccPower;
        public float AircontrolMaxSpeed => _aircontrolMaxSpeed;
        public float CoyoteTime => coyoteTime;

        #endregion

        #region WallJump

        [Header("WallJump Param")]

        [SerializeField] private AnimationCurve wallJumpHorizontalCurve;
        [SerializeField] private float wallJumpHorizontalDistance;

        [SerializeField] private AnimationCurve wallJumpVerticalCurve;
        [SerializeField] private float wallJumpVerticalDistance;

        [SerializeField] private float wallJumpTime;

        public JumpConfig WallJumpConfig
        {
            get => new JumpConfig(wallJumpTime, wallJumpVerticalCurve, wallJumpVerticalDistance, wallJumpHorizontalCurve, wallJumpHorizontalDistance);
        }

        [SerializeField] private float wallJumpInertia;
        public float WallJumpInertia => wallJumpInertia;

        #endregion

        #region WallSlide

        [Header("WallSlide Param")]

        [SerializeField] private float _wallSlideSpeedDown;
        [SerializeField] private float _wallSlideMaxSpeedDown;
        [SerializeField] private float _wallSlideMaxSpeedUp;
        [SerializeField] private float _wallSlideSpeedRatio;

        [SerializeField] private float timeToGetOff;
        [SerializeField] private float timeToGetOffOppositeDirection;
        [SerializeField] private float _wallSlideGetOffForce;
        [SerializeField] private float _wallSlideCoyoteTime;

        public float WallSlideSpeedDown => _wallSlideSpeedDown;
        public float WallSlideMaxSpeedDown => _wallSlideMaxSpeedDown;
        public float WallSlideMaxSpeedUp => _wallSlideMaxSpeedUp;
        public float WallSlideSpeedRatio => _wallSlideSpeedRatio;

        public float WallSlideGetOffForce => _wallSlideGetOffForce;
        public float TimeToGetOff => timeToGetOff;
        public float TimeToGetOffOppositeDirection => timeToGetOffOppositeDirection;
        public float WallSlideCoyoteTime => _wallSlideCoyoteTime;

        #endregion
    }
}