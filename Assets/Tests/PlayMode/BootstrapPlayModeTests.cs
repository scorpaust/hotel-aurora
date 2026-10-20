using System.Collections;
using Aurora.Core;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Aurora.Tests.PlayMode
{
    public class BootstrapPlayModeTests
    {
        [UnityTest]
        public IEnumerator PlayFromAnywhere_LoadsBootAndStartsPlaying()
        {
            yield return null;

            Assert.IsTrue(Services.TryGet<IGameStateService>(out var gameState));
            Assert.AreEqual(GameState.Playing, gameState.CurrentState);

            var bootScene = SceneManager.GetSceneByName("Boot");
            Assert.IsTrue(bootScene.IsValid());
            Assert.IsTrue(bootScene.isLoaded);
        }
    }
}
