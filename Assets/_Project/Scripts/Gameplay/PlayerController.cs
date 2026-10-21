#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Animator))]
    public sealed class PlayerController : MonoBehaviour
    {
        private static readonly int SpeedParam = Animator.StringToHash("Speed");
        private static readonly int IsMovingParam = Animator.StringToHash("IsMoving");
        private static readonly int IsRunningParam = Animator.StringToHash("IsRunning");
        private static readonly int IsCrouchingParam = Animator.StringToHash("IsCrouching");
        private static readonly int OnStairsParam = Animator.StringToHash("OnStairs");
        private static readonly int IsPushingParam = Animator.StringToHash("IsPushing");

        // Keeps a small gap between the collider and whatever it casts against, so the next
        // cast never starts flush against a wall. Physics2D.queriesStartInColliders is off
        // project-wide (see Physics2DSettings), which means a cast whose origin already
        // touches a collider ignores it entirely — without this margin, a kinematic box that
        // comes to rest exactly against a thin (edge-only) collider, like a Tilemap wall, can
        // slip through on the very next cast.
        private const float SkinWidth = 0.02f;

        // How far Move's vertical axis has to be pushed to count as "wants to climb/leave a
        // stair" — matches spec item 5 ("Move vertical entra na escada"): touching a StairZone
        // alone must never hijack plain horizontal walking through its footprint.
        private const float StairIntentDeadzone = 0.3f;

        [SerializeField] private PlayerMovementConfig _config = null!;
        [SerializeField] private LayerMask _collisionMask;

        [Header("Crouch")]
        [Tooltip("Crouching collider height as a fraction of the standing height.")]
        [SerializeField] private float _crouchColliderHeightScale = 0.6f;

        private Rigidbody2D _rigidbody = null!;
        private BoxCollider2D _collider = null!;
        private Animator _animator = null!;
        private PlayerMotor _motor = null!;
        private IInputService? _input;

        private Vector2 _standingColliderSize;
        private Vector2 _standingColliderOffset;

        /// <summary>The StairZone the player's collider currently physically overlaps, if any — not necessarily being climbed.</summary>
        private StairZone? _overlappingStairZone;

        /// <summary>The StairZone currently being climbed (a subset of overlapping: requires vertical intent to engage).</summary>
        private StairZone? _activeStairZone;

        private bool _isPushing;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            _motor = new PlayerMotor(_config);

            _standingColliderSize = _collider.size;
            _standingColliderOffset = _collider.offset;
        }

        private void Update()
        {
            if (_input == null && !Services.TryGet(out _input))
                return;

            if (!Services.TryGet<IGameStateService>(out var gameState) || !IsControllable(gameState.CurrentState))
                return;

            var move = _input.Gameplay.Move.ReadValue<Vector2>();

            if (_activeStairZone != null)
            {
                TickOnStairs(_activeStairZone, move.x, move.y);
            }
            else if (_overlappingStairZone != null && Mathf.Abs(move.y) > StairIntentDeadzone)
            {
                _activeStairZone = _overlappingStairZone;
                _motor.EnterStairs(_activeStairZone.ProjectProgress(_rigidbody.position));
                TickOnStairs(_activeStairZone, move.x, move.y);
            }
            else
            {
                var runHeld = _input.Gameplay.Run.IsPressed();
                var crouchHeld = _input.Gameplay.Crouch.IsPressed();
                var interactHeld = _input.Gameplay.Interact.IsPressed();
                var canStand = CanStandUp();

                _motor.Tick(move.x, runHeld, crouchHeld, canStand, Time.deltaTime);

                ApplyColliderShape();
                ApplyMovement(interactHeld);
            }

            UpdateAnimator();
        }

        private static bool IsControllable(GameState state)
        {
            return state == GameState.Playing || state == GameState.Chase;
        }

        /// <summary>
        /// Checks only the headroom band standing would add above crouch height — not the
        /// whole standing box — so something at chest height in front of the player (like a
        /// Pushable) never gets mistaken for a low ceiling overhead.
        /// </summary>
        private bool CanStandUp()
        {
            var crouchHeight = _standingColliderSize.y * _crouchColliderHeightScale;
            var headroomHeight = _standingColliderSize.y - crouchHeight;
            if (headroomHeight <= 0f)
                return true;

            var standingBottom = _standingColliderOffset.y - _standingColliderSize.y / 2f;
            var headroomCenterY = standingBottom + crouchHeight + headroomHeight / 2f;

            var origin = _rigidbody.position + new Vector2(_standingColliderOffset.x, headroomCenterY);
            var size = new Vector2(_standingColliderSize.x, headroomHeight);
            return Physics2D.OverlapBox(origin, size, 0f, _collisionMask) == null;
        }

        private void ApplyColliderShape()
        {
            if (_motor.IsCrouching)
            {
                var crouchSize = new Vector2(_standingColliderSize.x, _standingColliderSize.y * _crouchColliderHeightScale);
                var crouchOffset = _standingColliderOffset - new Vector2(0f, (_standingColliderSize.y - crouchSize.y) / 2f);
                _collider.size = crouchSize;
                _collider.offset = crouchOffset;
            }
            else
            {
                _collider.size = _standingColliderSize;
                _collider.offset = _standingColliderOffset;
            }
        }

        private void ApplyMovement(bool interactHeld)
        {
            _isPushing = false;

            var delta = _motor.Velocity * Time.deltaTime;
            if (delta == Vector2.zero)
                return;

            var distance = delta.magnitude;
            var direction = delta.normalized;
            var origin = _rigidbody.position + _collider.offset;
            var hit = Physics2D.BoxCast(origin, _collider.size, 0f, direction, distance + SkinWidth, _collisionMask);

            if (hit.collider != null && interactHeld && !_motor.IsCrouching &&
                hit.collider.TryGetComponent<Pushable>(out var pushable))
            {
                var moved = pushable.Push(direction, distance / pushable.Mass);
                if (moved > 0f)
                {
                    _rigidbody.MovePosition(_rigidbody.position + direction * moved);
                    _isPushing = true;
                }

                return;
            }

            var allowedDistance = hit.collider != null ? Mathf.Max(0f, hit.distance - SkinWidth) : distance;
            if (allowedDistance > 0f)
                _rigidbody.MovePosition(_rigidbody.position + direction * allowedDistance);
        }

        private void TickOnStairs(StairZone zone, float moveX, float moveY)
        {
            _isPushing = false;

            // Horizontal intent always means "let me off" — from anywhere on the stair, not
            // just exactly at an end. Climb progress rarely lands on exactly 0/1 (frame timing
            // varies), so gating sideways-exit on an exact clamp left the player stuck mid-climb,
            // unresponsive to A/D, whenever they let go of Up/Down without reaching the tip.
            if (Mathf.Abs(moveX) > StairIntentDeadzone)
            {
                _activeStairZone = null;
                _motor.ExitStairs();
                return;
            }

            _motor.TickOnStairs(moveY, zone.ClimbSpeed, zone.Length, Time.deltaTime);

            // Still pushing further past the top/bottom anchor also means the player wants off.
            var atTop = _motor.StairProgress >= 1f;
            var atBottom = _motor.StairProgress <= 0f;

            if ((atTop && moveY > 0f) || (atBottom && moveY < 0f))
            {
                _activeStairZone = null;
                _motor.ExitStairs();
                return;
            }

            _rigidbody.MovePosition(zone.PositionAt(_motor.StairProgress));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<StairZone>(out var zone))
                _overlappingStairZone = zone;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent<StairZone>(out var zone) || zone != _overlappingStairZone)
                return;

            _overlappingStairZone = null;

            if (_activeStairZone == zone)
            {
                _activeStairZone = null;
                _motor.ExitStairs();
            }
        }

        private void UpdateAnimator()
        {
            _animator.SetFloat(SpeedParam, Mathf.Abs(_motor.Velocity.x));
            _animator.SetBool(IsMovingParam, _motor.IsMoving);
            _animator.SetBool(IsRunningParam, _motor.IsRunning);
            _animator.SetBool(IsCrouchingParam, _motor.IsCrouching);
            _animator.SetBool(OnStairsParam, _motor.IsOnStairs);
            _animator.SetBool(IsPushingParam, _isPushing);

            if (!_motor.IsOnStairs)
            {
                var scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * (_motor.FacingRight ? 1f : -1f);
                transform.localScale = scale;
            }
        }
    }
}
