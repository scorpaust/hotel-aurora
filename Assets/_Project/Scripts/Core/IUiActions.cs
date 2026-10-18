using UnityEngine.InputSystem;

namespace Aurora.Core
{
    public interface IUiActions
    {
        InputAction Navigate { get; }
        InputAction Submit { get; }
        InputAction Cancel { get; }
        InputAction Back { get; }
        InputAction TabLeft { get; }
        InputAction TabRight { get; }
        InputAction Point { get; }
        InputAction Click { get; }
        InputAction ScrollWheel { get; }
        InputAction RightClick { get; }
        InputAction MiddleClick { get; }
    }
}
