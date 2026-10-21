using System.Collections;
using Aurora.Core;
using Aurora.Gameplay;
using Aurora.Input;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Aurora.Tests.PlayMode
{
    public class PlayerControllerTests : InputTestFixture
    {
        private const string PrefabPath = "Assets/_Project/Prefabs/Characters/PFB_TomasPreview.prefab";

        private Keyboard _keyboard = null!;
        private GameObject _instance = null!;

        public override void Setup()
        {
            base.Setup();

            _keyboard = InputSystem.AddDevice<Keyboard>();

            // GameBootstrap's InputService was created before this fixture reset/isolated the
            // Input System for the test; re-register a fresh one so its bindings resolve
            // against the devices this fixture just added.
            Services.Register<IInputService>(new InputService());

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            _instance = Object.Instantiate(prefab);
        }

        public override void TearDown()
        {
            if (_instance != null)
                Object.Destroy(_instance);

            base.TearDown();
        }

        [UnityTest]
        public IEnumerator HoldingMoveRight_AdvancesPositionOverTime()
        {
            Assert.IsTrue(Services.TryGet<IGameStateService>(out var gameState));
            Assert.AreEqual(GameState.Playing, gameState.CurrentState);

            var startX = _instance.transform.position.x;

            Press(_keyboard.dKey);
            for (var i = 0; i < 30; i++)
                yield return null;
            Release(_keyboard.dKey);

            Assert.Greater(_instance.transform.position.x, startX);
        }

        [UnityTest]
        public IEnumerator NoInput_DoesNotMove()
        {
            var startPosition = _instance.transform.position;

            for (var i = 0; i < 10; i++)
                yield return null;

            Assert.AreEqual(startPosition, (Vector3)_instance.transform.position);
        }

        [UnityTest]
        public IEnumerator HoldingMoveTowardThinWall_NeverTunnelsThrough()
        {
            const int environmentLayer = 10;

            var wallX = _instance.transform.position.x + 1f;
            var wall = new GameObject("TestThinWall") { layer = environmentLayer };
            var wallCollider = wall.AddComponent<BoxCollider2D>();
            wallCollider.size = new Vector2(0.05f, 2f);
            wall.transform.position = new Vector3(wallX, _instance.transform.position.y, 0f);

            Press(_keyboard.dKey);
            // Walking at ~2.2 u/s for 2s covers ~4.4 units — far more than the 1-unit gap,
            // enough to prove a tunneling regression if the skin-width fix ever regresses.
            for (var i = 0; i < 120; i++)
                yield return null;
            Release(_keyboard.dKey);

            Object.Destroy(wall);

            Assert.LessOrEqual(_instance.transform.position.x, wallX);
        }

        [UnityTest]
        public IEnumerator UnderLowCeiling_ForcesCrouchEvenWithoutCrouchInput()
        {
            const int environmentLayer = 10;
            var animator = _instance.GetComponent<Animator>();
            var playerCollider = _instance.GetComponent<BoxCollider2D>();
            var standingTopY = _instance.transform.position.y + playerCollider.offset.y + playerCollider.size.y / 2f;

            var ceiling = new GameObject("TestCeiling") { layer = environmentLayer };
            var ceilingCollider = ceiling.AddComponent<BoxCollider2D>();
            ceilingCollider.size = new Vector2(2f, 0.3f);
            ceiling.transform.position = new Vector3(_instance.transform.position.x, standingTopY, 0f);

            for (var i = 0; i < 5; i++)
                yield return null;

            Object.Destroy(ceiling);

            Assert.IsTrue(animator.GetBool("IsCrouching"));
        }

        [UnityTest]
        public IEnumerator ClimbingLadderToTop_ThenContinuingUp_ExitsCleanlyOntoGround()
        {
            const int triggerLayer = 11;
            var animator = _instance.GetComponent<Animator>();
            var startPos = _instance.transform.position;

            var zoneObject = new GameObject("TestLadder") { layer = triggerLayer, tag = "Stairs" };
            var zoneCollider = zoneObject.AddComponent<BoxCollider2D>();
            zoneCollider.isTrigger = true;
            zoneCollider.size = new Vector2(1f, 2.2f);
            zoneObject.transform.position = startPos + new Vector3(0f, 1f, 0f);
            var zone = zoneObject.AddComponent<StairZone>();
            var so = new SerializedObject(zone);
            so.FindProperty("_kind").enumValueIndex = (int)StairKind.Ladder;
            so.FindProperty("_bottomLocalOffset").vector2Value = new Vector2(0f, -1f);
            so.FindProperty("_topLocalOffset").vector2Value = new Vector2(0f, 1f);
            so.FindProperty("_climbSpeed").floatValue = 4f;
            so.ApplyModifiedPropertiesWithoutUndo();

            Press(_keyboard.wKey);
            // 2 units at 4 u/s = 0.5s (~30 frames) to reach the top, plus margin to confirm the
            // clean exit (still holding Up past the top should not leave the player stuck there).
            for (var i = 0; i < 90; i++)
                yield return null;
            Release(_keyboard.wKey);

            Object.Destroy(zoneObject);

            Assert.IsFalse(animator.GetBool("OnStairs"), "Should have exited the stair at the top instead of staying pinned there.");
            Assert.Greater(_instance.transform.position.y, startPos.y + 1.5f);
        }

        [UnityTest]
        public IEnumerator HoldingInteractAndMove_PushesAPushableBox()
        {
            const int environmentLayer = 10;

            var boxObject = new GameObject("TestPushableBox") { layer = environmentLayer };
            boxObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            var boxCollider = boxObject.AddComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(0.4f, 0.4f);
            boxObject.transform.position = new Vector3(_instance.transform.position.x + 0.5f, _instance.transform.position.y, 0f);
            var pushable = boxObject.AddComponent<Pushable>();
            var so = new SerializedObject(pushable);
            so.FindProperty("_mass").floatValue = 1f;
            so.FindProperty("_collisionMask").intValue = 1 << environmentLayer;
            so.ApplyModifiedPropertiesWithoutUndo();

            var startBoxX = boxObject.transform.position.x;

            Press(_keyboard.eKey);
            yield return null;
            Press(_keyboard.dKey);
            for (var i = 0; i < 60; i++)
                yield return null;
            Release(_keyboard.dKey);
            Release(_keyboard.eKey);

            var movedBoxX = boxObject.transform.position.x;
            Object.Destroy(boxObject);

            Assert.Greater(movedBoxX, startBoxX);
        }

        [UnityTest]
        public IEnumerator HoldingMoveWithoutInteract_DoesNotPushABox()
        {
            const int environmentLayer = 10;

            var boxObject = new GameObject("TestPushableBoxNoInteract") { layer = environmentLayer };
            boxObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            var boxCollider = boxObject.AddComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(0.4f, 0.4f);
            boxObject.transform.position = new Vector3(_instance.transform.position.x + 0.5f, _instance.transform.position.y, 0f);
            var pushable = boxObject.AddComponent<Pushable>();
            var so = new SerializedObject(pushable);
            so.FindProperty("_mass").floatValue = 1f;
            so.FindProperty("_collisionMask").intValue = 1 << environmentLayer;
            so.ApplyModifiedPropertiesWithoutUndo();

            var startBoxX = boxObject.transform.position.x;

            Press(_keyboard.dKey);
            for (var i = 0; i < 60; i++)
                yield return null;
            Release(_keyboard.dKey);

            var movedBoxX = boxObject.transform.position.x;
            Object.Destroy(boxObject);

            Assert.AreEqual(startBoxX, movedBoxX, 0.001f, "Without Interact held, the box should just block the player like a wall.");
        }
    }
}
