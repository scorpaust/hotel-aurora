#nullable enable

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Aurora.Editor.Build
{
    /// <summary>
    /// CLI-callable build entry points (`-executeMethod Aurora.Editor.Build.BuildRunner.BuildX`).
    /// Each writes to Builds/&lt;profile&gt;/&lt;version&gt;/ and a buildinfo.json alongside it,
    /// then exits batchmode with code 1 on failure so CI can detect it.
    /// </summary>
    public static class BuildRunner
    {
        private const string WebGLTemplate = "PROJECT:AuroraMinimal";

        [MenuItem("Aurora/Build/Web")]
        public static void BuildWeb() => RunWeb(development: false);

        [MenuItem("Aurora/Build/Web (Dev)")]
        public static void BuildWebDev() => RunWeb(development: true);

        [MenuItem("Aurora/Build/Windows")]
        public static void BuildWindows() => RunWindows(development: false, steam: false);

        [MenuItem("Aurora/Build/Windows (Dev)")]
        public static void BuildWindowsDev() => RunWindows(development: true, steam: false);

        [MenuItem("Aurora/Build/Windows (Steam)")]
        public static void BuildSteam() => RunWindows(development: false, steam: true);

        private static void RunWeb(bool development)
        {
            var originalTemplate = PlayerSettings.WebGL.template;
            var originalCompression = PlayerSettings.WebGL.compressionFormat;
            var originalFallback = PlayerSettings.WebGL.decompressionFallback;
            var originalExceptions = PlayerSettings.WebGL.exceptionSupport;

            try
            {
                PlayerSettings.WebGL.template = WebGLTemplate;
                // TODO F0-04: spec asks for Brotli, but it hung this Editor/Unity combo for
                // over an hour on the data-file compression step (0% CPU, "Not Responding").
                // Using Gzip until that's root-caused — revisit before shipping (F7-01).
                PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
                PlayerSettings.WebGL.decompressionFallback = true;
                PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.ExplicitlyThrownExceptionsOnly;

                Run("Web", BuildTarget.WebGL, development, extraDefines: null);
            }
            finally
            {
                PlayerSettings.WebGL.template = originalTemplate;
                PlayerSettings.WebGL.compressionFormat = originalCompression;
                PlayerSettings.WebGL.decompressionFallback = originalFallback;
                PlayerSettings.WebGL.exceptionSupport = originalExceptions;
            }
        }

        private static void RunWindows(bool development, bool steam)
        {
            var profileName = steam ? "Windows-Steam" : "Windows";
            var extraDefines = steam ? new[] { "STEAM_BUILD" } : null;
            Run(profileName, BuildTarget.StandaloneWindows64, development, extraDefines);
        }

        private static void Run(string profileName, BuildTarget target, bool development, string[]? extraDefines)
        {
            var cliArgs = Environment.GetCommandLineArgs();
            var version = BuildArgsParser.ResolveVersion(cliArgs, PlayerSettings.bundleVersion);
            var outputRoot = BuildArgsParser.ResolveOutputPath(cliArgs, profileName, version);
            var executableName = PlayerSettings.productName + (target == BuildTarget.WebGL ? "" : ".exe");
            var locationPathName = target == BuildTarget.WebGL
                ? outputRoot
                : Path.Combine(outputRoot, executableName);

            var namedBuildTarget = target == BuildTarget.WebGL
                ? UnityEditor.Build.NamedBuildTarget.WebGL
                : UnityEditor.Build.NamedBuildTarget.Standalone;

            var previousDefines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);

            if (extraDefines is { Length: > 0 })
            {
                var merged = previousDefines.Split(';')
                    .Concat(extraDefines)
                    .Where(d => !string.IsNullOrWhiteSpace(d))
                    .Distinct()
                    .ToArray();
                PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, string.Join(";", merged));
            }

            var options = new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
                locationPathName = locationPathName,
                target = target,
                options = development ? BuildOptions.Development : BuildOptions.None
            };

            BuildReport report;
            try
            {
                report = BuildPipeline.BuildPlayer(options);
            }
            finally
            {
                if (extraDefines is { Length: > 0 })
                    PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, previousDefines);
            }

            if (report.summary.result != BuildResult.Succeeded)
            {
                Debug.LogError($"[BuildRunner] {profileName} build failed: {report.summary.result} " +
                                $"({report.summary.totalErrors} error(s)).");
                FailBatchmode();
                return;
            }

            WriteBuildInfo(outputRoot, version);
            Debug.Log($"[BuildRunner] {profileName} build succeeded: {locationPathName} " +
                      $"({report.summary.totalSize / 1024 / 1024} MB).");
        }

        private static void WriteBuildInfo(string outputRoot, string version)
        {
            Directory.CreateDirectory(outputRoot);

            var commit = TryGetGitCommit() ?? "unknown";
            var json = "{\n" +
                       $"  \"version\": \"{version}\",\n" +
                       $"  \"commit\": \"{commit}\",\n" +
                       $"  \"builtAtUtc\": \"{DateTime.UtcNow:O}\"\n" +
                       "}\n";

            File.WriteAllText(Path.Combine(outputRoot, "buildinfo.json"), json);
        }

        private static string? TryGetGitCommit()
        {
            try
            {
                using var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "rev-parse --short HEAD",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process == null)
                    return null;

                var output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit(2000);
                return string.IsNullOrEmpty(output) ? null : output;
            }
            catch
            {
                return null;
            }
        }

        private static void FailBatchmode()
        {
            if (UnityEngine.Application.isBatchMode)
                EditorApplication.Exit(1);
        }
    }
}
