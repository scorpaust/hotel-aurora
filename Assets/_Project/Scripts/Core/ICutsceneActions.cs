#nullable enable

using UnityEngine.InputSystem;

namespace Aurora.Core
{
    public interface ICutsceneActions
    {
        InputAction Skip { get; }
    }
}
