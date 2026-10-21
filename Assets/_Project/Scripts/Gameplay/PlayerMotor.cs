#nullable enable

using UnityEngine;

namespace Aurora.Gameplay
{
    /// <summary>
    /// Pure, testable movement logic for Tomas. Has no Unity physics/scene dependency;
    /// PlayerController applies <see cref="Velocity"/> to a Rigidbody2D each frame.
    /// </summary>
    public sealed class PlayerMotor
    {
        private readonly PlayerMovementConfig _config;

        private float _currentSpeedX;
        private float _breathRemaining;

        public PlayerMotor(PlayerMovementConfig config)
        {
            _config = config;
            _breathRemaining = config.BreathDuration;
        }

        public Vector2 Velocity { get; private set; }
        public bool IsMoving { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsCrouching { get; private set; }
        public bool FacingRight { get; private set; } = true;
        public float BreathFraction01 { get; private set; } = 1f;

        /// <param name="moveX">Horizontal input, expected in [-1, 1].</param>
        /// <param name="runHeld">Run action held.</param>
        /// <param name="crouchHeld">Crouch action held.</param>
        /// <param name="canStand">False when a ceiling (conduct/low furniture) prevents standing up.</param>
        public void Tick(float moveX, bool runHeld, bool crouchHeld, bool canStand, float deltaTime)
        {
            IsCrouching = crouchHeld || !canStand;

            var wantsToMove = Mathf.Abs(moveX) > 0.0001f;
            var wantsToRun = runHeld && !IsCrouching && wantsToMove;

            if (wantsToRun && _breathRemaining > 0f)
                _breathRemaining = Mathf.Max(0f, _breathRemaining - deltaTime);
            else if (!wantsToRun)
                _breathRemaining = Mathf.Min(_config.BreathDuration, _breathRemaining + _config.BreathRecoveryRate * deltaTime);

            IsRunning = wantsToRun && _breathRemaining > 0f;
            BreathFraction01 = _config.BreathDuration > 0f ? _breathRemaining / _config.BreathDuration : 1f;

            var targetSpeed = IsCrouching ? _config.CrouchSpeed : IsRunning ? _config.RunSpeed : _config.WalkSpeed;
            var targetVelocityX = wantsToMove ? Mathf.Sign(moveX) * targetSpeed : 0f;

            var smoothTime = Mathf.Abs(targetVelocityX) > Mathf.Abs(_currentSpeedX)
                ? _config.AccelerationTime
                : _config.DecelerationTime;
            var maxDelta = smoothTime > 0f
                ? Mathf.Max(targetSpeed, _config.WalkSpeed) / smoothTime * deltaTime
                : float.MaxValue;
            _currentSpeedX = Mathf.MoveTowards(_currentSpeedX, targetVelocityX, maxDelta);

            IsMoving = Mathf.Abs(_currentSpeedX) > 0.01f;
            if (wantsToMove)
                FacingRight = moveX > 0f;

            Velocity = new Vector2(_currentSpeedX, 0f);
        }
    }
}
