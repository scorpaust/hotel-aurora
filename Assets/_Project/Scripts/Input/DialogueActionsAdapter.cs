using Aurora.Core;
using UnityEngine.InputSystem;

namespace Aurora.Input
{
    internal sealed class DialogueActionsAdapter : IDialogueActions
    {
        private readonly AuroraInputActions.DialogueActions _actions;

        public DialogueActionsAdapter(AuroraInputActions.DialogueActions actions)
        {
            _actions = actions;
        }

        public InputAction Advance => _actions.Advance;
        public InputAction Skip => _actions.Skip;
        public InputAction ChooseUp => _actions.ChooseUp;
        public InputAction ChooseDown => _actions.ChooseDown;
    }
}
