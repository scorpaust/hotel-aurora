#nullable enable

using UnityEngine;

namespace Aurora.Gameplay
{
    /// <summary>
    /// A light object Tomas can push by holding Interact while walking into it. Has its own
    /// kinematic BoxCast so it stops against walls/other obstacles instead of clipping through.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class Pushable : MonoBehaviour
    {
        // Same reasoning as PlayerController's SkinWidth: Physics2D.queriesStartInColliders is
        // off project-wide, so a cast whose origin already touches a collider ignores it — once
        // the box came to rest exactly flush against something (zero gap), the very next push
        // would miss that collider entirely and slide straight into/through it. Casting a little
        // further and clamping the allowed distance keeps a permanent sliver of a gap instead.
        private const float SkinWidth = 0.02f;

        [Tooltip("Higher mass pushes slower: push speed is divided by this.")]
        [SerializeField] private float _mass = 1f;

        [SerializeField] private LayerMask _collisionMask;

        private Rigidbody2D _rigidbody = null!;
        private BoxCollider2D _collider = null!;

        public float Mass => Mathf.Max(0.01f, _mass);

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
        }

        /// <summary>Tries to move by up to <paramref name="distance"/> along <paramref name="direction"/>; stops early if blocked. Returns the distance actually moved.</summary>
        public float Push(Vector2 direction, float distance)
        {
            if (distance <= 0f)
                return 0f;

            var origin = _rigidbody.position + _collider.offset;
            var hit = Physics2D.BoxCast(origin, _collider.size, 0f, direction, distance + SkinWidth, _collisionMask);
            var allowed = hit.collider != null ? Mathf.Max(0f, hit.distance - SkinWidth) : distance;

            if (allowed > 0f)
                _rigidbody.MovePosition(_rigidbody.position + direction * allowed);

            return allowed;
        }
    }
}
