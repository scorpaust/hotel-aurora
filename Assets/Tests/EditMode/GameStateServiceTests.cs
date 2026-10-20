using Aurora.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Aurora.Tests.EditMode
{
    public class GameStateServiceTests
    {
        private sealed class FakeInputService : IInputService
        {
            public IGameplayActions Gameplay => null!;
            public IUiActions UI => null!;
            public IDialogueActions Dialogue => null!;
            public ICutsceneActions Cutscene => null!;
            public InputContext CurrentContext { get; private set; }
            public InputDeviceKind CurrentDevice => InputDeviceKind.KeyboardMouse;
            public event System.Action<InputDeviceKind>? DeviceChanged;

            public void SetContext(InputContext context)
            {
                CurrentContext = context;
            }
        }

        [TearDown]
        public void TearDown()
        {
            Services.Reset();
            Time.timeScale = 1f;
        }

        [Test]
        public void InitialState_IsBoot()
        {
            var service = new GameStateService();

            Assert.AreEqual(GameState.Boot, service.CurrentState);
        }

        [Test]
        public void SetState_ValidTransition_Succeeds()
        {
            var service = new GameStateService();

            service.SetState(GameState.MainMenu);

            Assert.AreEqual(GameState.MainMenu, service.CurrentState);
        }

        [Test]
        public void SetState_InvalidTransition_IsRejectedAndLogsError()
        {
            var service = new GameStateService();
            LogAssert.Expect(LogType.Error, "[GameStateService] Invalid transition: Boot -> Chase.");

            service.SetState(GameState.Chase);

            Assert.AreEqual(GameState.Boot, service.CurrentState);
        }

        [Test]
        public void SetState_Paused_SetsTimeScaleToZero()
        {
            var service = new GameStateService();
            service.SetState(GameState.Playing);

            service.SetState(GameState.Paused);

            Assert.AreEqual(0f, Time.timeScale);
        }

        [Test]
        public void SetState_ResumeFromPause_RestoresTimeScale()
        {
            var service = new GameStateService();
            service.SetState(GameState.Playing);
            service.SetState(GameState.Paused);

            service.SetState(GameState.Playing);

            Assert.AreEqual(1f, Time.timeScale);
        }

        [Test]
        public void SetState_SameState_IsNoOpAndDoesNotRaiseEvent()
        {
            var service = new GameStateService();
            service.SetState(GameState.MainMenu);
            var raised = false;
            service.StateChanged += _ => raised = true;

            service.SetState(GameState.MainMenu);

            Assert.IsFalse(raised);
        }

        [Test]
        public void SetState_Dialogue_SwitchesRegisteredInputServiceToDialogueContext()
        {
            var fakeInput = new FakeInputService();
            Services.Register<IInputService>(fakeInput);
            var service = new GameStateService();
            service.SetState(GameState.MainMenu);
            service.SetState(GameState.Playing);

            service.SetState(GameState.Dialogue);

            Assert.AreEqual(InputContext.Dialogue, fakeInput.CurrentContext);
        }

        [Test]
        public void SetState_Pause_SwitchesRegisteredInputServiceToPauseContext()
        {
            var fakeInput = new FakeInputService();
            Services.Register<IInputService>(fakeInput);
            var service = new GameStateService();
            service.SetState(GameState.MainMenu);
            service.SetState(GameState.Playing);

            service.SetState(GameState.Paused);

            Assert.AreEqual(InputContext.Pause, fakeInput.CurrentContext);
        }
    }
}
