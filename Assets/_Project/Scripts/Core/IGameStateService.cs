#nullable enable

using System;

namespace Aurora.Core
{
    public interface IGameStateService
    {
        GameState CurrentState { get; }
        void SetState(GameState state);
        event Action<GameState> StateChanged;
    }
}
