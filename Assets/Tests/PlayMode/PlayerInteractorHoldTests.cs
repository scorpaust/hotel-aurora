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
    // F1-03 spec "Testes": Hold completo vs cancelado. Driven through InputTestFixture rather
    // than a live Editor session — simulating a sustained key hold against a running Play
    // session needs real OS window focus (the Input System gates action-phase processing on
    // it), which an automated/headless driver does not have; a test fixture has no such gate.
    public class PlayerInteractorHoldTests : InputTestFixture
    {
        private const string PrefabPath = "Assets/_Project/Prefabs/Characters/PFB_TomasPreview.prefab";
        private const string FlagKey = "test.hold.switch.flag";
        private const float HoldDuration = 0.5f;

        private Keyboard _keyboard = null!;
        private GameObject _instance = null!;
        private GameObject _switchObject = null!;

        public override void Setup()
        {
            base.Setup();

            _keyboard = InputSystem.AddDevice<Keyboard>();
            Services.Register<IInputService>(new InputService());

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            _instance = Object.Instantiate(prefab);

            _switchObject = new GameObject("TestHoldSwitch");
            _switchObject.transform.position = _instance.transform.position + new Vector3(0.5f, 0f, 0f);
            var collider = _switchObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(0.5f, 1.6f);

            var sw = _switchObject.AddComponent<Switch>();
            var so = new SerializedObject(sw);
            so.FindProperty("_flagKey").stringValue = FlagKey;
            so.FindProperty("_kind").enumValueIndex = (int)InteractionKind.Hold;
            so.FindProperty("_holdDuration").floatValue = HoldDuration;
            so.ApplyModifiedPropertiesWithoutUndo();

            Services.TryGet<IWorldFlagsService>(out var flags);
            flags.SetFlag(FlagKey, -1);
        }

        public override void TearDown()
        {
            if (_switchObject != null)
                Object.Destroy(_switchObject);
            if (_instance != null)
                Object.Destroy(_instance);

            base.TearDown();
        }

        [UnityTest]
        public IEnumerator HoldingLessThanDuration_ThenReleasing_Cancels()
        {
            // Let PlayerInteractor's throttled (10 Hz) retarget pick up the switch first.
            for (var i = 0; i < 10; i++)
                yield return null;

            Press(_keyboard.eKey);
            for (var i = 0; i < 15; i++) // ~0.25s at 60 fps — comfortably under the 0.5s duration
                yield return null;
            Release(_keyboard.eKey);
            yield return null;

            Services.TryGet<IWorldFlagsService>(out var flags);
            Assert.AreEqual(-1, flags.GetFlag(FlagKey, -1), "Releasing before the hold duration must not fire Interact().");

            var interactor = _instance.GetComponent<PlayerInteractor>();
            Assert.AreEqual(0f, interactor.HoldProgress01, "Progress should reset to 0 once released.");
        }

        [UnityTest]
        public IEnumerator HoldingPastDuration_Completes()
        {
            for (var i = 0; i < 10; i++)
                yield return null;

            Press(_keyboard.eKey);
            for (var i = 0; i < 40; i++) // ~0.67s at 60 fps — comfortably over the 0.5s duration
                yield return null;
            Release(_keyboard.eKey);

            Services.TryGet<IWorldFlagsService>(out var flags);
            Assert.AreEqual(1, flags.GetFlag(FlagKey, -1), "Holding past the duration must fire Interact() exactly once.");
        }
    }
}
