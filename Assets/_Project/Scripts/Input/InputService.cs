using System;
using Aurora.Core;
using UnityEngine.InputSystem;

namespace Aurora.Input
{
    public sealed class InputService : IInputService, IDisposable
    {
        private readonly AuroraInputActions _actions;
        private readonly GameplayActionsAdapter _gameplay;
        private readonly UiActionsAdapter _ui;
        private readonly DialogueActionsAdapter _dialogue;
        private readonly CutsceneActionsAdapter _cutscene;

        public IGameplayActions Gameplay => _gameplay;
        public IUiActions UI => _ui;
        public IDialogueActions Dialogue => _dialogue;
        public ICutsceneActions Cutscene => _cutscene;

        public InputContext CurrentContext { get; private set; }
        public InputDeviceKind CurrentDevice { get; private set; }

        public event Action<InputDeviceKind> DeviceChanged;

        public InputService()
        {
            _actions = new AuroraInputActions();
            _gameplay = new GameplayActionsAdapter(_actions.Gameplay);
            _ui = new UiActionsAdapter(_actions.UI);
            _dialogue = new DialogueActionsAdapter(_actions.Dialogue);
            _cutscene = new CutsceneActionsAdapter(_actions.Cutscene);

            CurrentDevice = InputDeviceKind.KeyboardMouse;

            InputSystem.onActionChange += OnActionChange;

            SetContext(InputContext.Gameplay);
        }

        public void SetContext(InputContext context)
        {
            _actions.Gameplay.Disable();
            _actions.UI.Disable();
            _actions.Dialogue.Disable();
            _actions.Cutscene.Disable();

            switch (context)
            {
                case InputContext.Gameplay:
                    _actions.Gameplay.Enable();
                    break;
                case InputContext.UI:
                    _actions.UI.Enable();
                    break;
                case InputContext.Dialogue:
                    _actions.Dialogue.Enable();
                    break;
                case InputContext.Cutscene:
                    _actions.Cutscene.Enable();
                    break;
                case InputContext.Pause:
                    _actions.Gameplay.Enable();
                    _actions.UI.Enable();
                    break;
            }

            CurrentContext = context;
        }

        public void Dispose()
        {
            InputSystem.onActionChange -= OnActionChange;

            _actions.Gameplay.Disable();
            _actions.UI.Disable();
            _actions.Dialogue.Disable();
            _actions.Cutscene.Disable();
            _actions.Dispose();
        }

        private void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.ActionPerformed || obj is not InputAction action)
                return;

            var device = action.activeControl?.device;
            if (device == null)
                return;

            var kind = ClassifyDevice(device);
            if (kind == null || kind == CurrentDevice)
                return;

            CurrentDevice = kind.Value;
            DeviceChanged?.Invoke(CurrentDevice);
        }

        private static InputDeviceKind? ClassifyDevice(InputDevice device)
        {
            if (device is Keyboard || device is Mouse)
                return InputDeviceKind.KeyboardMouse;

            if (device is Gamepad)
            {
                var name = device.displayName ?? string.Empty;

                if (name.IndexOf("Steam Deck", StringComparison.OrdinalIgnoreCase) >= 0)
                    return InputDeviceKind.SteamDeck;
                if (name.IndexOf("DualShock", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("DualSense", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("PlayStation", StringComparison.OrdinalIgnoreCase) >= 0)
                    return InputDeviceKind.PlayStation;
                if (name.IndexOf("Xbox", StringComparison.OrdinalIgnoreCase) >= 0)
                    return InputDeviceKind.Xbox;

                return InputDeviceKind.GenericGamepad;
            }

            return null;
        }
    }
}
