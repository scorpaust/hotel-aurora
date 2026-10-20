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
    }
}
