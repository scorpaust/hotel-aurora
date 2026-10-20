using Aurora.Editor;
using Aurora.Hotel;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Aurora.Tests.EditMode
{
    public class LevelFlowTests
    {
        [Test]
        public void Validate_NoBeats_IsRejected()
        {
            var flow = ScriptableObject.CreateInstance<LevelFlowDefinition>();
            try
            {
                var so = new SerializedObject(flow);
                so.FindProperty("_id").stringValue = "levelflow.test.empty";
                so.ApplyModifiedPropertiesWithoutUndo();

                var result = LevelFlowValidator.Validate(flow);

                Assert.IsFalse(result.Passed);
                Assert.That(result.Issues[0], Does.Contain("no beats"));
            }
            finally
            {
                Object.DestroyImmediate(flow);
            }
        }

        [Test]
        public void Validate_EnemyEncounterWithoutEnemies_IsRejected()
        {
            var flow = ScriptableObject.CreateInstance<LevelFlowDefinition>();
            try
            {
                var so = new SerializedObject(flow);
                so.FindProperty("_id").stringValue = "levelflow.test.missing-enemies";
                var beats = so.FindProperty("_beats");
                beats.arraySize = 1;
                beats.GetArrayElementAtIndex(0).FindPropertyRelative("_type").enumValueIndex =
                    (int)LevelBeatType.EnemyEncounter;
                so.ApplyModifiedPropertiesWithoutUndo();

                var result = LevelFlowValidator.Validate(flow);

                Assert.IsFalse(result.Passed);
                Assert.That(result.Issues[0], Does.Contain("EnemyEncounter").And.Contain("EnemyDefinition"));
            }
            finally
            {
                Object.DestroyImmediate(flow);
            }
        }

        [Test]
        public void Validate_CutsceneWithoutReference_IsRejected()
        {
            var flow = ScriptableObject.CreateInstance<LevelFlowDefinition>();
            try
            {
                var so = new SerializedObject(flow);
                so.FindProperty("_id").stringValue = "levelflow.test.missing-cutscene";
                var beats = so.FindProperty("_beats");
                beats.arraySize = 1;
                beats.GetArrayElementAtIndex(0).FindPropertyRelative("_type").enumValueIndex =
                    (int)LevelBeatType.Cutscene;
                so.ApplyModifiedPropertiesWithoutUndo();

                var result = LevelFlowValidator.Validate(flow);

                Assert.IsFalse(result.Passed);
                Assert.That(result.Issues[0], Does.Contain("CutsceneDefinition"));
            }
            finally
            {
                Object.DestroyImmediate(flow);
            }
        }

        [Test]
        public void Validate_ExplorationBeatNeedsNoReference_Passes()
        {
            var flow = ScriptableObject.CreateInstance<LevelFlowDefinition>();
            try
            {
                var so = new SerializedObject(flow);
                so.FindProperty("_id").stringValue = "levelflow.test.exploration-only";
                var beats = so.FindProperty("_beats");
                beats.arraySize = 1;
                beats.GetArrayElementAtIndex(0).FindPropertyRelative("_type").enumValueIndex =
                    (int)LevelBeatType.Exploration;
                so.ApplyModifiedPropertiesWithoutUndo();

                var result = LevelFlowValidator.Validate(flow);

                Assert.IsTrue(result.Passed);
            }
            finally
            {
                Object.DestroyImmediate(flow);
            }
        }

        [Test]
        public void LevelFlowDefinition_RoundTripsBeatOrderThroughSerialization()
        {
            var flow = ScriptableObject.CreateInstance<LevelFlowDefinition>();
            try
            {
                var so = new SerializedObject(flow);
                so.FindProperty("_id").stringValue = "levelflow.test.order";

                var beats = so.FindProperty("_beats");
                beats.arraySize = 3;

                var beat0 = beats.GetArrayElementAtIndex(0);
                beat0.FindPropertyRelative("_type").enumValueIndex = (int)LevelBeatType.Cutscene;
                beat0.FindPropertyRelative("_label").stringValue = "Abertura";

                var beat1 = beats.GetArrayElementAtIndex(1);
                beat1.FindPropertyRelative("_type").enumValueIndex = (int)LevelBeatType.Exploration;
                beat1.FindPropertyRelative("_label").stringValue = "Meio";

                var beat2 = beats.GetArrayElementAtIndex(2);
                beat2.FindPropertyRelative("_type").enumValueIndex = (int)LevelBeatType.Boss;
                beat2.FindPropertyRelative("_label").stringValue = "Fim";

                so.ApplyModifiedPropertiesWithoutUndo();

                Assert.AreEqual("levelflow.test.order", flow.Id);
                Assert.AreEqual(3, flow.Beats.Count);
                Assert.AreEqual(LevelBeatType.Cutscene, flow.Beats[0].Type);
                Assert.AreEqual("Abertura", flow.Beats[0].Label);
                Assert.AreEqual(LevelBeatType.Exploration, flow.Beats[1].Type);
                Assert.AreEqual("Meio", flow.Beats[1].Label);
                Assert.AreEqual(LevelBeatType.Boss, flow.Beats[2].Type);
                Assert.AreEqual("Fim", flow.Beats[2].Label);
            }
            finally
            {
                Object.DestroyImmediate(flow);
            }
        }
    }
}
