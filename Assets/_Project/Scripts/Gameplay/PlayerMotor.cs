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

        /// <summary>True while climbing a <see cref="StairZone"/>; ground Tick/collision don't run.</summary>
        public bool IsOnStairs { get; private set; }

        /// <summary>0 at the stair's bottom anchor, 1 at its top anchor.</summary>
        public float StairProgress { get; private set; }

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
            // Scaled by the input's own magnitude (not just its sign) so an analog stick can
            // walk slower than full speed; keyboard/d-pad input is always -1/0/1, so this is a
            // no-op for them and both devices share the exact same code path.
            var targetVelocityX = wantsToMove ? Mathf.Clamp(moveX, -1f, 1f) * targetSpeed : 0f;

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

        /// <summary>Starts climbing a stair zone at the given progress (0 = bottom, 1 = top).</summary>
        public void EnterStairs(float initialProgress)
        {
            IsOnStairs = true;
            StairProgress = Mathf.Clamp01(initialProgress);
            Velocity = Vector2.zero;
        }

        /// <summary>
        /// Advances <see cref="StairProgress"/> from vertical input alone — horizontal input is
        /// ignored while on a stair, so degraus and escadas de mão share the same traversal:
        /// position is Lerp(bottom, top, StairProgress) on whatever line the zone defines
        /// (vertical for a ladder, diagonal for a flight of steps). Never collision-tested, so
        /// it can never get stuck mid-climb.
        /// </summary>
        /// <param name="moveY">Vertical input, expected in [-1, 1]; positive climbs toward the top.</param>
        /// <param name="climbSpeed">Units/second along the zone's bottom-to-top segment.</param>
        /// <param name="segmentLength">World length of that segment.</param>
        public void TickOnStairs(float moveY, float climbSpeed, float segmentLength, float deltaTime)
        {
            IsMoving = Mathf.Abs(moveY) > 0.0001f;
            IsRunning = false;

            if (segmentLength > 0.0001f)
            {
                var deltaProgress = Mathf.Clamp(moveY, -1f, 1f) * climbSpeed * deltaTime / segmentLength;
                StairProgress = Mathf.Clamp01(StairProgress + deltaProgress);
            }

            Velocity = Vector2.zero;
        }

        public void ExitStairs()
        {
            IsOnStairs = false;
        }
    }
}
