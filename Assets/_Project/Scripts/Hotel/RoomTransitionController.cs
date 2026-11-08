#nullable enable

using System.Threading;
using Aurora.Core;
using Aurora.Gameplay;
using Unity.Cinemachine;
using UnityEngine;

namespace Aurora.Hotel
{
    /// <summary>
    /// Orchestrates a RoomConnection activation (F1-02 spec item 3): blocks input, fades out,
    /// resolves the (possibly flag-conditional) destination, loads/unloads the ala scene via
    /// ISceneService if needed, warps the player, swaps the camera confiner, fades back in.
    /// </summary>
    public sealed class RoomTransitionController : MonoBehaviour, IRoomTransitionService
    {
        [SerializeField] private CinemachineConfiner2D? _confiner;
        [SerializeField] private GameEvent? _fadeOutRequested;
        [SerializeField] private GameEvent? _fadeInRequested;
        [SerializeField] private float _fadeDuration = 0.25f;

        private string? _currentAlaSceneName;

        public bool IsTransitioning { get; private set; }

        private void Start()
        {
            Services.Register<IRoomTransitionService>(this);
        }

        /// <summary>Editor-authoring helper — lets tooling set these private fields without SerializedObject.</summary>
        public void Configure(CinemachineConfiner2D? confiner, GameEvent? fadeOutRequested, GameEvent? fadeInRequested, float fadeDuration)
        {
            _confiner = confiner;
            _fadeOutRequested = fadeOutRequested;
            _fadeInRequested = fadeInRequested;
            _fadeDuration = fadeDuration;
        }

        public async Awaitable RequestTransitionAsync(RoomTransitionRequest request, CancellationToken cancellationToken = default)
        {
            if (IsTransitioning)
                return;

            if (!Services.TryGet<IGameStateService>(out var gameState))
                return;

            IsTransitioning = true;
            gameState.SetState(GameState.Transition);

            var destination = ResolveDestination(request);

            _fadeOutRequested?.Raise();
            await Awaitable.WaitForSecondsAsync(_fadeDuration, cancellationToken);

            if (destination.IsCrossScene && Services.TryGet<ISceneService>(out var sceneService))
                await sceneService.LoadAdditiveAsync(destination.SceneName, cancellationToken);

            if (Room.TryGetById(destination.RoomId, out var room) &&
                room.TryGetSpawnPoint(destination.SpawnId, out var spawnPoint))
            {
                PlayerController.Current?.WarpTo(spawnPoint.transform.position);

                if (_confiner != null && room.Bounds != null)
                {
                    _confiner.BoundingShape2D = room.Bounds;
                    _confiner.InvalidateBoundingShapeCache();
                }

                await UnloadPreviousAlaIfLeftAsync(room, destination, cancellationToken);
            }
            else
            {
                Debug.LogError(
                    $"[RoomTransitionController] Could not resolve destination room '{destination.RoomId}' / spawn '{destination.SpawnId}'.");
            }

            _fadeInRequested?.Raise();
            await Awaitable.WaitForSecondsAsync(_fadeDuration, cancellationToken);

            gameState.SetState(GameState.Playing);
            IsTransitioning = false;
        }

        private async Awaitable UnloadPreviousAlaIfLeftAsync(
            Room arrivedRoom, RoomDestination destination, CancellationToken cancellationToken)
        {
            if (destination.IsCrossScene)
                _currentAlaSceneName = destination.SceneName;

            if (_currentAlaSceneName == null || arrivedRoom.gameObject.scene.name == _currentAlaSceneName)
                return;

            if (Services.TryGet<ISceneService>(out var sceneService))
                await sceneService.UnloadAsync(_currentAlaSceneName, cancellationToken);

            _currentAlaSceneName = null;
        }

        private static RoomDestination ResolveDestination(RoomTransitionRequest request)
        {
            var flagValue = Services.TryGet<IWorldFlagsService>(out var flags)
                ? flags.GetFlag(request.ConditionFlagKey)
                : 0;

            return RoomTransitionResolver.Resolve(request, flagValue);
        }
    }
}
