using Aurora.Editor.Build;
using NUnit.Framework;

namespace Aurora.Tests.EditMode
{
    public class BuildArgsParserTests
    {
        [Test]
        public void GetArg_PresentFlag_ReturnsFollowingValue()
        {
            var args = new[] { "Unity.exe", "-buildVersion", "1.2.3", "-batchmode" };

            Assert.AreEqual("1.2.3", BuildArgsParser.GetArg(args, "-buildVersion"));
        }

        [Test]
        public void GetArg_MissingFlag_ReturnsNull()
        {
            var args = new[] { "Unity.exe", "-batchmode" };

            Assert.IsNull(BuildArgsParser.GetArg(args, "-buildVersion"));
        }

        [Test]
        public void GetArg_FlagIsLastArgument_ReturnsNull()
        {
            var args = new[] { "Unity.exe", "-buildVersion" };

            Assert.IsNull(BuildArgsParser.GetArg(args, "-buildVersion"));
        }

        [Test]
        public void ResolveVersion_NoArgGiven_FallsBackToProvidedVersion()
        {
            var args = new[] { "Unity.exe", "-batchmode" };

            Assert.AreEqual("0.0.1", BuildArgsParser.ResolveVersion(args, "0.0.1"));
        }

        [Test]
        public void ResolveVersion_ArgGiven_OverridesFallback()
        {
            var args = new[] { "-buildVersion", "2.0.0" };

            Assert.AreEqual("2.0.0", BuildArgsParser.ResolveVersion(args, "0.0.1"));
        }

        [Test]
        public void ResolveOutputPath_NoArgGiven_DefaultsUnderBuildsProfileVersion()
        {
            var args = new[] { "-batchmode" };

            var path = BuildArgsParser.ResolveOutputPath(args, "Web", "1.0.0");

            Assert.AreEqual(System.IO.Path.Combine("Builds", "Web", "1.0.0"), path);
        }

        [Test]
        public void ResolveOutputPath_ArgGiven_OverridesDefault()
        {
            var args = new[] { "-outputPath", "/tmp/custom-build" };

            var path = BuildArgsParser.ResolveOutputPath(args, "Web", "1.0.0");

            Assert.AreEqual("/tmp/custom-build", path);
        }
    }
}
