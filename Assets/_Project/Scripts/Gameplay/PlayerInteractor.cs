#nullable enable

using System.Collections.Generic;
using Aurora.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Aurora.Gameplay
{
    /// <summary>
    /// Finds the best nearby IInteractable (OverlapCircleNonAlloc, no per-frame allocations)
    /// and fires it through Interact (Press) or InteractHold (Hold). Re-evaluates the target
    /// at most 10 times/second (spec item 2) rather than every frame.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public sealed class PlayerInteractor : MonoBehaviour
    {
        private const float RetargetInterval = 1f / 10f;
        private const int MaxCandidates = 8;

        [SerializeField] private float _radius = 1f;
        [SerializeField] private Vector2 _forwardOffset = new(0.5f, 0f);
        [SerializeField] private LayerMask _interactableMask = ~0;

        private readonly Collider2D[] _overlapBuffer = new Collider2D[MaxCandidates];
        private readonly List<(IInteractable interactable, float distance)> _candidates = new();

        private PlayerController _player = null!;
        private IInputService? _input;
        private ContactFilter2D _filter;
        private float _retargetTimer;
        private float _holdElapsed;
        private bool _holdFired;

        public IInteractable? CurrentTarget { get; private set; }
        public float HoldProgress01 { get; private set; }

        private void Awake()
        {
            _player = GetComponent<PlayerController>();
            // Interactables (RoomConnection, etc.) sit on trigger colliders — useTriggers must
            // be explicit here rather than relying on the project-wide queriesHitTriggers default.
            _filter = new ContactFilter2D { useTriggers = true };
            _filter.SetLayerMask(_interactableMask);
        }

        private void Update()
        {
            if (_input == null && !Services.TryGet<IInputService>(out _input))
                return;

            _retargetTimer -= Time.deltaTime;
            if (_retargetTimer <= 0f)
            {
                _retargetTimer = RetargetInterval;
                Retarget();
            }

            TickCurrentTarget();
        }

        private Vector2 ProbeOrigin()
        {
            var offset = _player.FacingRight ? _forwardOffset : new Vector2(-_forwardOffset.x, _forwardOffset.y);
            return (Vector2)transform.position + offset;
        }

        private void Retarget()
        {
            var origin = ProbeOrigin();
            var count = Physics2D.OverlapCircle(origin, _radius, _filter, _overlapBuffer);

            _candidates.Clear();
            var context = new InteractionContext(gameObject);

            for (var i = 0; i < count; i++)
            {
                if (!_overlapBuffer[i].TryGetComponent<IInteractable>(out var interactable))
                    continue;
                if (!interactable.CanInteract(context))
                    continue;

                var distance = Vector2.Distance(origin, _overlapBuffer[i].ClosestPoint(origin));
                _candidates.Add((interactable, distance));
            }

            var newTarget = InteractableSelector.SelectBest(_candidates);
            if (ReferenceEquals(newTarget, CurrentTarget))
                return;

            CurrentTarget = newTarget;
            HoldProgress01 = 0f;
            _holdElapsed = 0f;
            _holdFired = false;
        }

        private void TickCurrentTarget()
        {
            if (CurrentTarget == null || _input == null)
                return;

            var context = new InteractionContext(gameObject);

            if (CurrentTarget.Kind == InteractionKind.Press)
            {
                if (_input.Gameplay.Interact.WasPressedThisFrame())
                    CurrentTarget.Interact(context);
                return;
            }

            // Per-target Hold duration: tracked by hand against IsPressed(), rather than the
            // Input System's own Hold interaction timeout. ApplyParameterOverride("hold:duration",
            // ...) is the documented API for this, but it does not change the interaction's
            // actual firing time in this project's Input System version — confirmed live,
            // including re-enabling the action/map and applying the override before the
            // first Enable() — so InteractHold carries no interaction at all; it is a plain
            // button and this method decides when a hold "performs" or "cancels".
            if (!_input.Gameplay.InteractHold.IsPressed())
            {
                _holdElapsed = 0f;
                HoldProgress01 = 0f;
                _holdFired = false;
                return;
            }

            _holdElapsed += Time.deltaTime;
            var duration = Mathf.Max(CurrentTarget.HoldDuration, 0.0001f);
            HoldProgress01 = Mathf.Clamp01(_holdElapsed / duration);

            if (!_holdFired && _holdElapsed >= duration)
            {
                _holdFired = true;
                CurrentTarget.Interact(context);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_player == null)
                _player = GetComponent<PlayerController>();

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Application.isPlaying ? ProbeOrigin() : (Vector2)transform.position + _forwardOffset, _radius);
        }
    }
}
