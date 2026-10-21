using Aurora.Gameplay;
using NUnit.Framework;
using UnityEngine;

namespace Aurora.Tests.EditMode
{
    public class StairZoneTests
    {
        private GameObject _gameObject = null!;
        private StairZone _zone = null!;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject("TestStairZone");
            _gameObject.AddComponent<BoxCollider2D>();
            _zone = _gameObject.AddComponent<StairZone>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_gameObject);
        }

        private static void SetAnchors(StairZone zone, GameObject go, Vector2 bottomOffset, Vector2 topOffset)
        {
            var so = new UnityEditor.SerializedObject(zone);
            so.FindProperty("_bottomLocalOffset").vector2Value = bottomOffset;
            so.FindProperty("_topLocalOffset").vector2Value = topOffset;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        [Test]
        public void Ladder_BottomAndTop_AreAtExpectedWorldPositions()
        {
            _gameObject.transform.position = new Vector3(2f, 1f, 0f);
            SetAnchors(_zone, _gameObject, new Vector2(0f, -1f), new Vector2(0f, 1f));

            Assert.AreEqual(new Vector2(2f, 0f), _zone.Bottom);
            Assert.AreEqual(new Vector2(2f, 2f), _zone.Top);
            Assert.AreEqual(2f, _zone.Length, 0.001f);
        }

        [Test]
        public void Steps_TopAndBottom_CanBeDiagonal()
        {
            SetAnchors(_zone, _gameObject, new Vector2(-1f, 0f), new Vector2(1f, 1f));

            Assert.AreEqual(Mathf.Sqrt(5f), _zone.Length, 0.001f);
        }

        [Test]
        public void ProjectProgress_AtBottom_ReturnsZero()
        {
            SetAnchors(_zone, _gameObject, new Vector2(0f, -1f), new Vector2(0f, 1f));

            Assert.AreEqual(0f, _zone.ProjectProgress(_zone.Bottom), 0.001f);
        }

        [Test]
        public void ProjectProgress_AtTop_ReturnsOne()
        {
            SetAnchors(_zone, _gameObject, new Vector2(0f, -1f), new Vector2(0f, 1f));

            Assert.AreEqual(1f, _zone.ProjectProgress(_zone.Top), 0.001f);
        }

        [Test]
        public void ProjectProgress_PastEitherEnd_IsClamped()
        {
            SetAnchors(_zone, _gameObject, new Vector2(0f, -1f), new Vector2(0f, 1f));

            Assert.AreEqual(0f, _zone.ProjectProgress(new Vector2(0f, -5f)), 0.001f);
            Assert.AreEqual(1f, _zone.ProjectProgress(new Vector2(0f, 5f)), 0.001f);
        }

        [Test]
        public void PositionAt_HalfProgress_IsMidpoint()
        {
            SetAnchors(_zone, _gameObject, new Vector2(-1f, 0f), new Vector2(1f, 2f));

            Assert.AreEqual(new Vector2(0f, 1f), _zone.PositionAt(0.5f));
        }
    }
}
