using Aurora.Core;
using UnityEngine.InputSystem;

namespace Aurora.Input
{
    internal sealed class UiActionsAdapter : IUiActions
    {
        private readonly AuroraInputActions.UIActions _actions;

        public UiActionsAdapter(AuroraInputActions.UIActions actions)
        {
            _actions = actions;
        }

        public InputAction Navigate => _actions.Navigate;
        public InputAction Submit => _actions.Submit;
        public InputAction Cancel => _actions.Cancel;
        public InputAction Back => _actions.Back;
        public InputAction TabLeft => _actions.TabLeft;
        public InputAction TabRight => _actions.TabRight;
        public InputAction Point => _actions.Point;
        public InputAction Click => _actions.Click;
        public InputAction ScrollWheel => _actions.ScrollWheel;
        public InputAction RightClick => _actions.RightClick;
        public InputAction MiddleClick => _actions.MiddleClick;
    }
}
