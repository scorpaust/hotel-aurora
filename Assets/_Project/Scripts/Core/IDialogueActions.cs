#nullable enable

using UnityEngine.InputSystem;

namespace Aurora.Core
{
    public interface IDialogueActions
    {
        InputAction Advance { get; }
        InputAction Skip { get; }
        InputAction ChooseUp { get; }
        InputAction ChooseDown { get; }
    }
}
