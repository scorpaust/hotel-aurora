#nullable enable

using UnityEngine.InputSystem;

namespace Aurora.Core
{
    public interface IGameplayActions
    {
        InputAction Move { get; }
        InputAction Run { get; }
        InputAction Crouch { get; }
        InputAction Interact { get; }
        InputAction Lantern { get; }
        InputAction LanternFocus { get; }
        InputAction UseTool { get; }
        InputAction CycleTool { get; }
        InputAction CallBeatriz { get; }
        InputAction Pause { get; }
    }
}
