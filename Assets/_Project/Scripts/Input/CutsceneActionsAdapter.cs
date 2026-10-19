using Aurora.Core;
using UnityEngine.InputSystem;

namespace Aurora.Input
{
    internal sealed class CutsceneActionsAdapter : ICutsceneActions
    {
        private readonly AuroraInputActions.CutsceneActions _actions;

        public CutsceneActionsAdapter(AuroraInputActions.CutsceneActions actions)
        {
            _actions = actions;
        }

        public InputAction Skip => _actions.Skip;
    }
}
