using Aurora.Core;
using NUnit.Framework;
using UnityEngine;

namespace Aurora.Tests.EditMode
{
    public class LanternConeTests
    {
        [Test]
        public void Contains_PointStraightAheadWithinRange_IsTrue()
        {
            var cone = new LanternCone(Vector2.zero, Vector2.right, 30f, 5f);

            Assert.IsTrue(cone.Contains(new Vector2(3f, 0f)));
        }

        [Test]
        public void Contains_PointBeyondRange_IsFalse()
        {
            var cone = new LanternCone(Vector2.zero, Vector2.right, 30f, 5f);

            Assert.IsFalse(cone.Contains(new Vector2(10f, 0f)));
        }

        [Test]
        public void Contains_PointOutsideHalfAngle_IsFalse()
        {
            var cone = new LanternCone(Vector2.zero, Vector2.right, 30f, 5f);

            // 45 degrees above the aim direction, outside a 30-degree half-angle.
            Assert.IsFalse(cone.Contains(new Vector2(1f, 1f)));
        }

        [Test]
        public void Contains_PointExactlyAtHalfAngleEdge_IsTrue()
        {
            var cone = new LanternCone(Vector2.zero, Vector2.right, 45f, 5f);

            Assert.IsTrue(cone.Contains(new Vector2(1f, 1f)));
        }

        [Test]
        public void Contains_PointBehindOrigin_IsFalse()
        {
            var cone = new LanternCone(Vector2.zero, Vector2.right, 30f, 5f);

            Assert.IsFalse(cone.Contains(new Vector2(-3f, 0f)));
        }

        [Test]
        public void Contains_OriginItself_IsTrue()
        {
            var cone = new LanternCone(new Vector2(2f, 2f), Vector2.right, 30f, 5f);

            Assert.IsTrue(cone.Contains(new Vector2(2f, 2f)));
        }

        [Test]
        public void Constructor_ZeroDirection_FallsBackToRight()
        {
            var cone = new LanternCone(Vector2.zero, Vector2.zero, 30f, 5f);

            Assert.IsTrue(cone.Contains(new Vector2(3f, 0f)));
        }
    }
}
