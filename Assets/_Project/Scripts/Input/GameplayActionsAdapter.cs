using Aurora.Core;
using UnityEngine.InputSystem;

namespace Aurora.Input
{
    internal sealed class GameplayActionsAdapter : IGameplayActions
    {
        private readonly AuroraInputActions.GameplayActions _actions;

        public GameplayActionsAdapter(AuroraInputActions.GameplayActions actions)
        {
            _actions = actions;
        }

        public InputAction Move => _actions.Move;
        public InputAction Run => _actions.Run;
        public InputAction Crouch => _actions.Crouch;
        public InputAction Interact => _actions.Interact;
        public InputAction Lantern => _actions.Lantern;
        public InputAction LanternFocus => _actions.LanternFocus;
        public InputAction UseTool => _actions.UseTool;
        public InputAction CycleTool => _actions.CycleTool;
        public InputAction CallBeatriz => _actions.CallBeatriz;
        public InputAction Pause => _actions.Pause;
    }
}
