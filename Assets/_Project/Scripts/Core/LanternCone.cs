using UnityEngine;

namespace Aurora.Core
{
    /// <summary>
    /// The lantern's revealing cone (F1-04 spec item 4) as a pure, testable shape — origin,
    /// aim direction, half-angle and range. Used to decide whether a PastLayerObject point is
    /// currently lit, independent of any shader/visual mask.
    /// </summary>
    public readonly struct LanternCone
    {
        public Vector2 Origin { get; }
        public Vector2 Direction { get; }
        public float HalfAngleDegrees { get; }
        public float Range { get; }

        public LanternCone(Vector2 origin, Vector2 direction, float halfAngleDegrees, float range)
        {
            Origin = origin;
            Direction = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right;
            HalfAngleDegrees = halfAngleDegrees;
            Range = range;
        }

        public bool Contains(Vector2 point)
        {
            var offset = point - Origin;
            var distance = offset.magnitude;
            if (distance > Range)
                return false;
            if (distance <= 0f)
                return true;

            return Vector2.Angle(Direction, offset) <= HalfAngleDegrees;
        }
    }
}
