using System.Collections;
using Aurora.Core;
using Aurora.Gameplay;
using Aurora.Hotel;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Aurora.Tests.PlayMode
{
    public class RoomTransitionControllerTests
    {
        private const string TomasPrefabPath = "Assets/_Project/Prefabs/Characters/PFB_TomasPreview.prefab";

        private GameObject _player = null!;
        private GameObject _roomA = null!;
        private GameObject _roomB = null!;
        private GameObject _controllerGo = null!;

        [SetUp]
        public void Setup()
        {
            Services.Reset();

            var gameState = new GameStateService();
            gameState.SetState(GameState.Playing);
            Services.Register<IGameStateService>(gameState);
            Services.Register<IWorldFlagsService>(new WorldFlagsService());

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(TomasPrefabPath);
            _player = Object.Instantiate(prefab, new Vector3(0f, 0.78f, 0f), Quaternion.identity);

            _roomA = BuildRoom("TestRoomA", "Spawn_A", Vector3.zero);
            _roomB = BuildRoom("TestRoomB", "Spawn_B", new Vector3(10f, 0f, 0f));

            _controllerGo = new GameObject("TransitionController");
            _controllerGo.AddComponent<RoomTransitionController>().Configure(null, null, null, 0.01f);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_player);
            Object.Destroy(_roomA);
            Object.Destroy(_roomB);
            Object.Destroy(_controllerGo);
            Services.Reset();
        }

        private static GameObject BuildRoom(string roomId, string spawnId, Vector3 origin)
        {
            var definition = ScriptableObject.CreateInstance<RoomDefinition>();
            definition.Configure(roomId, roomId);

            var roomGo = new GameObject($"Room_{roomId}") { transform = { position = origin } };
            var bounds = roomGo.AddComponent<BoxCollider2D>();
            roomGo.AddComponent<Room>().Configure(definition, bounds);

            var spawnGo = new GameObject($"Spawn_{roomId}");
            spawnGo.transform.SetParent(roomGo.transform);
            spawnGo.transform.localPosition = new Vector3(1f, 0f, 0f);
            spawnGo.AddComponent<RoomSpawnPoint>().SetId(spawnId);

            return roomGo;
        }

        [UnityTest]
        public IEnumerator Activate_UnconditionalConnection_WarpsPlayerToDestinationAndReturnsToPlaying()
        {
            var connectionGo = new GameObject("Connection");
            connectionGo.AddComponent<BoxCollider2D>();
            var connection = connectionGo.AddComponent<RoomConnection>();
            connection.Configure(RoomConnectionType.Door, new RoomDestination("TestRoomB", "Spawn_B"));

            connection.Activate();

            // Fade duration is 0.01s each way, but batchmode frame pacing varies a lot —
            // give it generous headroom (2s worth of frames) instead of a tight frame count.
            for (var i = 0; i < 120 && Services.Get<IGameStateService>().CurrentState != GameState.Playing; i++)
                yield return null;

            Assert.AreEqual(GameState.Playing, Services.Get<IGameStateService>().CurrentState);
            Assert.AreEqual(11f, _player.transform.position.x, 0.01f);

            Object.Destroy(connectionGo);
        }

        [UnityTest]
        public IEnumerator Activate_ConditionNotMet_WarpsPlayerToFallbackDestination()
        {
            var connectionGo = new GameObject("Connection");
            connectionGo.AddComponent<BoxCollider2D>();
            var connection = connectionGo.AddComponent<RoomConnection>();
            connection.Configure(
                RoomConnectionType.Door,
                new RoomDestination("TestRoomA", "Spawn_A"),
                "test.flag", 1,
                new RoomDestination("TestRoomB", "Spawn_B"));

            connection.Activate();

            for (var i = 0; i < 120 && Services.Get<IGameStateService>().CurrentState != GameState.Playing; i++)
                yield return null;

            Assert.AreEqual(11f, _player.transform.position.x, 0.01f);

            Object.Destroy(connectionGo);
        }
    }
}
