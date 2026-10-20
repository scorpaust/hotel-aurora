#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aurora.Core
{
    public sealed class GameStateService : IGameStateService
    {
        private static readonly Dictionary<GameState, GameState[]> Transitions = new()
        {
            [GameState.Boot] = new[] { GameState.MainMenu, GameState.Playing },
            [GameState.MainMenu] = new[] { GameState.Playing },
            [GameState.Playing] = new[]
            {
                GameState.Paused, GameState.Dialogue, GameState.Cutscene, GameState.Chase,
                GameState.GameOver, GameState.MainMenu
            },
            [GameState.Paused] = new[] { GameState.Playing, GameState.MainMenu },
            [GameState.Dialogue] = new[] { GameState.Playing },
            [GameState.Cutscene] = new[] { GameState.Playing },
            [GameState.Chase] = new[] { GameState.Playing, GameState.Paused, GameState.GameOver },
            [GameState.GameOver] = new[] { GameState.Playing, GameState.MainMenu }
        };

        private static readonly Dictionary<GameState, InputContext> ContextByState = new()
        {
            [GameState.Boot] = InputContext.UI,
            [GameState.MainMenu] = InputContext.UI,
            [GameState.Playing] = InputContext.Gameplay,
            [GameState.Paused] = InputContext.Pause,
            [GameState.Dialogue] = InputContext.Dialogue,
            [GameState.Cutscene] = InputContext.Cutscene,
            [GameState.Chase] = InputContext.Gameplay,
            [GameState.GameOver] = InputContext.UI
        };

        public GameState CurrentState { get; private set; } = GameState.Boot;

        public event Action<GameState>? StateChanged;

        public void SetState(GameState state)
        {
            if (state == CurrentState)
                return;

            if (!Transitions.TryGetValue(CurrentState, out var allowed) || Array.IndexOf(allowed, state) < 0)
            {
                Debug.LogError($"[GameStateService] Invalid transition: {CurrentState} -> {state}.");
                return;
            }

            CurrentState = state;
            Time.timeScale = state == GameState.Paused ? 0f : 1f;

            if (Services.TryGet<IInputService>(out var input))
                input.SetContext(ContextByState[state]);

            StateChanged?.Invoke(state);
        }
    }
}
