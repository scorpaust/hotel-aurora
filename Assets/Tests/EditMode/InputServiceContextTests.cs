using Aurora.Core;
using Aurora.Input;
using NUnit.Framework;

namespace Aurora.Tests.EditMode
{
    public class InputServiceContextTests
    {
        // Note: the generated AuroraInputActions.Dispose() calls UnityEngine.Object.Destroy(),
        // which is illegal in Edit Mode. These tests disable maps directly instead of disposing;
        // the underlying InputActionAsset is cleaned up on the next domain reload.

        [Test]
        public void Gameplay_IsActiveContext_ByDefault()
        {
            var service = new InputService();

            Assert.AreEqual(InputContext.Gameplay, service.CurrentContext);
            Assert.IsTrue(service.Gameplay.Move.enabled);
            Assert.IsFalse(service.UI.Navigate.enabled);
            Assert.IsFalse(service.Dialogue.Advance.enabled);
            Assert.IsFalse(service.Cutscene.Skip.enabled);

            service.SetContext(InputContext.Dialogue);
        }

        [Test]
        public void SetContext_Dialogue_DisablesGameplayMove()
        {
            var service = new InputService();

            service.SetContext(InputContext.Dialogue);

            Assert.IsFalse(service.Gameplay.Move.enabled);
            Assert.IsTrue(service.Dialogue.Advance.enabled);
        }

        [Test]
        public void SetContext_Gameplay_AfterDialogue_RestoresMove()
        {
            var service = new InputService();

            service.SetContext(InputContext.Dialogue);
            service.SetContext(InputContext.Gameplay);

            Assert.IsTrue(service.Gameplay.Move.enabled);
            Assert.IsFalse(service.Dialogue.Advance.enabled);
        }

        [Test]
        public void SetContext_Cutscene_OnlyEnablesCutsceneMap()
        {
            var service = new InputService();

            service.SetContext(InputContext.Cutscene);

            Assert.IsTrue(service.Cutscene.Skip.enabled);
            Assert.IsFalse(service.Gameplay.Move.enabled);
            Assert.IsFalse(service.UI.Navigate.enabled);
            Assert.IsFalse(service.Dialogue.Advance.enabled);

            service.SetContext(InputContext.Dialogue);
        }

        [Test]
        public void SetContext_Pause_EnablesGameplayAndUiTogether()
        {
            var service = new InputService();

            service.SetContext(InputContext.Pause);

            Assert.IsTrue(service.Gameplay.Move.enabled);
            Assert.IsTrue(service.UI.Navigate.enabled);
            Assert.IsFalse(service.Dialogue.Advance.enabled);
            Assert.IsFalse(service.Cutscene.Skip.enabled);

            service.SetContext(InputContext.Dialogue);
        }
    }
}
