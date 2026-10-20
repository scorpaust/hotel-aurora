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

        [SerializeField] private PlayerMovementConfig _config = null!;
        [SerializeField] private LayerMask _collisionMask;

        private Rigidbody2D _rigidbody = null!;
        private BoxCollider2D _collider = null!;
        private Animator _animator = null!;
        private PlayerMotor _motor = null!;
        private IInputService? _input;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            _motor = new PlayerMotor(_config);
        }

        private void Update()
        {
            if (_input == null && !Services.TryGet(out _input))
                return;

            if (!Services.TryGet<IGameStateService>(out var gameState) || !IsControllable(gameState.CurrentState))
                return;

            var move = _input.Gameplay.Move.ReadValue<Vector2>();
            var runHeld = _input.Gameplay.Run.IsPressed();
            var crouchHeld = _input.Gameplay.Crouch.IsPressed();

            _motor.Tick(move.x, runHeld, crouchHeld, canStand: true, Time.deltaTime);

            ApplyMovement();
            UpdateAnimator();
        }

        private static bool IsControllable(GameState state)
        {
            return state == GameState.Playing || state == GameState.Chase;
        }

        private void ApplyMovement()
        {
            var delta = _motor.Velocity * Time.deltaTime;
            if (delta == Vector2.zero)
                return;

            var distance = delta.magnitude;
            var direction = delta.normalized;
            var hit = Physics2D.BoxCast(_rigidbody.position, _collider.size, 0f, direction, distance, _collisionMask);
            var allowedDistance = hit.collider != null ? Mathf.Max(0f, hit.distance) : distance;

            if (allowedDistance > 0f)
                _rigidbody.MovePosition(_rigidbody.position + direction * allowedDistance);
        }

        private void UpdateAnimator()
        {
            _animator.SetFloat(SpeedParam, Mathf.Abs(_motor.Velocity.x));
            _animator.SetBool(IsMovingParam, _motor.IsMoving);
            _animator.SetBool(IsRunningParam, _motor.IsRunning);
            _animator.SetBool(IsCrouchingParam, _motor.IsCrouching);

            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (_motor.FacingRight ? 1f : -1f);
            transform.localScale = scale;
        }
    }
}
