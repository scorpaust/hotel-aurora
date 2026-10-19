#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Input
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        private const string MainMenuSceneName = "MainMenu";

        private async void Awake()
        {
            Services.Reset();

            Services.Register<IInputService>(new InputService());
            var gameStateService = new GameStateService();
            Services.Register<IGameStateService>(gameStateService);
            var sceneService = new SceneService();
            Services.Register<ISceneService>(sceneService);

            if (BootstrapFlags.PlayFromAnywhere)
            {
                BootstrapFlags.PlayFromAnywhere = false;
                gameStateService.SetState(GameState.Playing);
                return;
            }

            await sceneService.LoadAdditiveAsync(MainMenuSceneName, destroyCancellationToken);
            gameStateService.SetState(GameState.MainMenu);
        }
    }
}
