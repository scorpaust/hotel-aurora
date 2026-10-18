using System.IO;
using Aurora.Editor;
using NUnit.Framework;

namespace Aurora.Tests.EditMode
{
    public class ProjectValidatorTests
    {
        private string _tempDir = "";

        [SetUp]
        public void SetUp()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "AuroraProjectValidatorTests_" + Path.GetRandomFileName());
            Directory.CreateDirectory(_tempDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, recursive: true);
        }

        [Test]
        public void FindLegacyInputUsages_DetectsLegacyInputGetKey()
        {
            var filePath = Path.Combine(_tempDir, "LegacyInputUser.cs");
            File.WriteAllText(filePath,
                "using UnityEngine;\n\npublic class LegacyInputUser\n{\n    void Update()\n    {\n        if (Input.GetKey(KeyCode.Space)) { }\n    }\n}\n");

            var matches = ProjectValidator.FindLegacyInputUsages(_tempDir);

            Assert.That(matches, Has.Count.EqualTo(1));
        }

        [Test]
        public void FindLegacyInputUsages_IgnoresInputSystemCode()
        {
            var filePath = Path.Combine(_tempDir, "InputSystemUser.cs");
            File.WriteAllText(filePath,
                "using UnityEngine.InputSystem;\n\npublic class InputSystemUser\n{\n    void OnMove(InputAction.CallbackContext ctx) { }\n}\n");

            var matches = ProjectValidator.FindLegacyInputUsages(_tempDir);

            Assert.That(matches, Is.Empty);
        }
    }
}
