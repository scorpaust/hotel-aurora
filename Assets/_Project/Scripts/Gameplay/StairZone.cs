#nullable enable

using UnityEngine;

namespace Aurora.Gameplay
{
    public enum StairKind
    {
        Steps,
        Ladder
    }

    /// <summary>
    /// A climbable path between a bottom and top anchor — vertical for an escada de mão
    /// (ladder), diagonal for a flight of degraus. PlayerController detects overlap via the
    /// trigger collider and moves the player directly along the anchor line while inside,
    /// bypassing wall collision entirely (a short, designer-placed path never gets stuck).
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class StairZone : MonoBehaviour
    {
        [SerializeField] private StairKind _kind = StairKind.Steps;
        [SerializeField] private Vector2 _bottomLocalOffset = new(0f, -0.5f);
        [SerializeField] private Vector2 _topLocalOffset = new(0f, 0.5f);
        [SerializeField] private float _climbSpeed = 2f;

        private void Reset()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        public StairKind Kind => _kind;
        public float ClimbSpeed => _climbSpeed;
        public Vector2 Bottom => (Vector2)transform.position + _bottomLocalOffset;
        public Vector2 Top => (Vector2)transform.position + _topLocalOffset;
        public float Length => Vector2.Distance(Bottom, Top);

        /// <summary>Projects a world position onto the Bottom→Top segment, clamped to 0..1.</summary>
        public float ProjectProgress(Vector2 worldPosition)
        {
            var segment = Top - Bottom;
            var lengthSquared = segment.sqrMagnitude;
            if (lengthSquared < 0.0001f)
                return 0f;

            var t = Vector2.Dot(worldPosition - Bottom, segment) / lengthSquared;
            return Mathf.Clamp01(t);
        }

        public Vector2 PositionAt(float progress) => Vector2.Lerp(Bottom, Top, Mathf.Clamp01(progress));
    }
}
