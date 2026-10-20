using System.Collections.Generic;
using Aurora.Core;
using Aurora.Editor;
using Aurora.Hotel;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Aurora.Tests.EditMode
{
    public class ContentDefinitionValidatorTests
    {
        private sealed class FakeDefinition : IContentDefinition
        {
            public FakeDefinition(string id)
            {
                Id = id;
            }

            public string Id { get; }
        }

        [Test]
        public void Validate_NoCandidates_Passes()
        {
            var result = ContentDefinitionValidator.Validate(new List<(string, IContentDefinition)>());

            Assert.IsTrue(result.Passed);
            Assert.IsEmpty(result.Issues);
        }

        [Test]
        public void Validate_DistinctIdsWithinType_Passes()
        {
            var candidates = new List<(string, IContentDefinition)>
            {
                ("a.asset", new FakeDefinition("enemy.zombie")),
                ("b.asset", new FakeDefinition("enemy.eyeguy"))
            };

            var result = ContentDefinitionValidator.Validate(candidates);

            Assert.IsTrue(result.Passed);
        }

        [Test]
        public void Validate_DuplicateIdWithinSameType_IsRejected()
        {
            var candidates = new List<(string, IContentDefinition)>
            {
                ("a.asset", new FakeDefinition("enemy.zombie")),
                ("b.asset", new FakeDefinition("enemy.zombie"))
            };

            var result = ContentDefinitionValidator.Validate(candidates);

            Assert.IsFalse(result.Passed);
            Assert.That(result.Issues[0], Does.Contain("Duplicate").And.Contain("enemy.zombie"));
        }

        [Test]
        public void Validate_EmptyId_IsRejected()
        {
            var candidates = new List<(string, IContentDefinition)>
            {
                ("a.asset", new FakeDefinition(string.Empty))
            };

            var result = ContentDefinitionValidator.Validate(candidates);

            Assert.IsFalse(result.Passed);
            Assert.That(result.Issues[0], Does.Contain("empty Id"));
        }

        [Test]
        public void BossDefinition_RoundTripsPhasesThroughSerialization()
        {
            var boss = ScriptableObject.CreateInstance<BossDefinition>();
            try
            {
                var so = new SerializedObject(boss);
                so.FindProperty("_id").stringValue = "boss.governanta";

                var phases = so.FindProperty("_phases");
                phases.arraySize = 2;

                var phase0 = phases.GetArrayElementAtIndex(0);
                phase0.FindPropertyRelative("_name").stringValue = "Perseguição";
                phase0.FindPropertyRelative("_lanternPuzzleDescription").stringValue = "Revelar o rosto no espelho";
                phase0.FindPropertyRelative("_transitionCondition").stringValue = "quartos.governanta.vista";

                var phase1 = phases.GetArrayElementAtIndex(1);
                phase1.FindPropertyRelative("_name").stringValue = "Confronto final";
                phase1.FindPropertyRelative("_lanternPuzzleDescription").stringValue = "Manter a luz até ela parar";
                phase1.FindPropertyRelative("_transitionCondition").stringValue = "quartos.governanta.libertada";

                so.ApplyModifiedPropertiesWithoutUndo();

                Assert.AreEqual("boss.governanta", boss.Id);
                Assert.AreEqual(2, boss.Phases.Count);
                Assert.AreEqual("Perseguição", boss.Phases[0].Name);
                Assert.AreEqual("Revelar o rosto no espelho", boss.Phases[0].LanternPuzzleDescription);
                Assert.AreEqual("Confronto final", boss.Phases[1].Name);
                Assert.AreEqual("quartos.governanta.libertada", boss.Phases[1].TransitionCondition);
            }
            finally
            {
                Object.DestroyImmediate(boss);
            }
        }
    }
}
