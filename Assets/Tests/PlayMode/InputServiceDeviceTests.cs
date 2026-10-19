using System.Collections;
using Aurora.Core;
using Aurora.Input;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Aurora.Tests.PlayMode
{
    public class InputServiceDeviceTests : InputTestFixture
    {
        private InputService _service;
        private Keyboard _keyboard;
        private Gamepad _gamepad;

        public override void Setup()
        {
            base.Setup();

            _keyboard = InputSystem.AddDevice<Keyboard>();
            _gamepad = InputSystem.AddDevice<Gamepad>();
            _service = new InputService();
        }

        public override void TearDown()
        {
            ((System.IDisposable)_service).Dispose();
            base.TearDown();
        }

        [UnityTest]
        public IEnumerator Move_RespondsToKeyboardWasd()
        {
            Press(_keyboard.dKey);
            yield return null;

            Assert.Greater(_service.Gameplay.Move.ReadValue<Vector2>().x, 0f);

            Release(_keyboard.dKey);
            yield return null;
        }

        [UnityTest]
        public IEnumerator DeviceChanged_FiresOnce_WhenSwitchingFromKeyboardToGamepad()
        {
            PressAndRelease(_keyboard.wKey);
            yield return null;

            Assert.AreEqual(InputDeviceKind.KeyboardMouse, _service.CurrentDevice);

            var changeCount = 0;
            InputDeviceKind? lastKind = null;
            _service.DeviceChanged += kind =>
            {
                changeCount++;
                lastKind = kind;
            };

            Set(_gamepad.leftStick, new Vector2(1f, 0f));
            yield return null;

            Assert.AreEqual(1, changeCount);
            Assert.AreEqual(InputDeviceKind.GenericGamepad, lastKind);
            Assert.AreEqual(InputDeviceKind.GenericGamepad, _service.CurrentDevice);

            Set(_gamepad.leftStick, Vector2.zero);
            yield return null;
        }
    }
}
