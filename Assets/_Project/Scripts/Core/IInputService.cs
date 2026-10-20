#nullable enable

using System;

namespace Aurora.Core
{
    public interface IInputService
    {
        IGameplayActions Gameplay { get; }
        IUiActions UI { get; }
        IDialogueActions Dialogue { get; }
        ICutsceneActions Cutscene { get; }

        InputContext CurrentContext { get; }
        void SetContext(InputContext context);

        InputDeviceKind CurrentDevice { get; }
        event Action<InputDeviceKind> DeviceChanged;
    }
}
