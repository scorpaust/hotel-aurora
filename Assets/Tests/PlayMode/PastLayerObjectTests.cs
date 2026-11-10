using System.Collections;
using System.Reflection;
using Aurora.Core;
using Aurora.Gameplay;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Aurora.Tests.PlayMode
{
    // F1-04 spec item 4 ("PhysicalWhenRevealed") / Testes: "revelação física liga/desliga o
    // collider". Drives LanternController's Cone/IsRevealing directly via their backing
    // fields rather than through real input — this is about PastLayerObject reacting to the
    // lantern's state, not about re-testing input simulation (see F1-03's PlayerInteractorHoldTests
    // for why that needs InputTestFixture instead).
    public class PastLayerObjectTests
    {
        private const string PrefabPath = "Assets/_Project/Prefabs/Characters/PFB_TomasPreview.prefab";
        private const string PermanentFlagKey = "test.pastlayer.permanent.flag";

        private GameObject _player = null!;
        private GameObject _target = null!;
        private LanternController _lantern = null!;
        private PastLayerObject _pastLayer = null!;
        private BoxCollider2D _physicalCollider = null!;

        [SetUp]
        public void Setup()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            _player = Object.Instantiate(prefab);
            _lantern = _player.GetComponent<LanternController>();

            _target = new GameObject("TestPastLayerTarget");
            _target.AddComponent<SpriteRenderer>();
            _physicalCollider = _target.AddComponent<BoxCollider2D>();
            _physicalCollider.enabled = false;

            _pastLayer = _target.AddComponent<PastLayerObject>();
            var so = new SerializedObject(_pastLayer);
            so.FindProperty("_physicalWhenRevealed").boolValue = true;
            so.FindProperty("_physicalCollider").objectReferenceValue = _physicalCollider;
            so.FindProperty("_graceSeconds").floatValue = 0.3f;
            so.FindProperty("_secondsToBecomePermanent").floatValue = 0.3f;
            so.FindProperty("_permanentFlagKey").stringValue = PermanentFlagKey;
            so.ApplyModifiedPropertiesWithoutUndo();

            Services.TryGet<IWorldFlagsService>(out var flags);
            flags.SetFlag(PermanentFlagKey, -1);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_player);
            Object.Destroy(_target);
        }

        private void SetLit(bool lit)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var coneField = typeof(LanternController).GetField("<Cone>k__BackingField", flags);
            var revealingField = typeof(LanternController).GetField("<IsRevealing>k__BackingField", flags);

            revealingField!.SetValue(_lantern, lit);
            coneField!.SetValue(_lantern, new LanternCone(_target.transform.position, Vector2.right, 180f, lit ? 100f : 0f));
            _lantern.enabled = false; // freeze this snapshot; real Update() would overwrite it
        }

        [UnityTest]
        public IEnumerator WhenLit_ColliderEnables()
        {
            SetLit(true);
            yield return null;

            Assert.IsTrue(_physicalCollider.enabled);
        }

        [UnityTest]
        public IEnumerator WhenNotLit_ColliderStaysDisabled()
        {
            SetLit(false);
            yield return null;

            Assert.IsFalse(_physicalCollider.enabled);
        }

        [UnityTest]
        public IEnumerator AfterLeavingLight_StaysEnabledDuringGrace_ThenDisables()
        {
            SetLit(true);
            yield return null;
            Assert.IsTrue(_physicalCollider.enabled);

            SetLit(false);
            yield return null;
            Assert.IsTrue(_physicalCollider.enabled, "Should still be enabled during the grace window.");

            yield return new WaitForSeconds(0.4f); // past the 0.3s grace

            Assert.IsFalse(_physicalCollider.enabled);
        }

        [UnityTest]
        public IEnumerator HoldingLitPastThreshold_BecomesPermanentAndSetsFlag()
        {
            SetLit(true);
            yield return new WaitForSeconds(0.4f); // past the 0.3s threshold

            Services.TryGet<IWorldFlagsService>(out var flags);
            Assert.AreEqual(1, flags.GetFlag(PermanentFlagKey, -1));

            SetLit(false);
            yield return null;

            Assert.IsTrue(_physicalCollider.enabled,
                "A permanently revealed object must stay revealed even once the lantern moves away.");
        }

        [UnityTest]
        public IEnumerator LeavingLightBeforeThreshold_DoesNotBecomePermanent()
        {
            SetLit(true);
            yield return new WaitForSeconds(0.1f); // well under the 0.3s threshold

            SetLit(false);
            yield return new WaitForSeconds(0.1f);

            Services.TryGet<IWorldFlagsService>(out var flags);
            Assert.AreEqual(-1, flags.GetFlag(PermanentFlagKey, -1));
        }
    }
}
