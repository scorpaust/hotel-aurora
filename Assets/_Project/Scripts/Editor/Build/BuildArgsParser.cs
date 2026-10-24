#nullable enable

using System.IO;

namespace Aurora.Editor.Build
{
    /// <summary>
    /// Pure parsing of -executeMethod command-line arguments into build paths/version — kept
    /// separate from BuildRunner so it's testable without invoking an actual build.
    /// </summary>
    public static class BuildArgsParser
    {
        public static string? GetArg(string[] args, string name)
        {
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == name)
                    return args[i + 1];
            }

            return null;
        }

        public static string ResolveVersion(string[] args, string fallbackVersion)
        {
            return GetArg(args, "-buildVersion") ?? fallbackVersion;
        }

        public static string ResolveOutputPath(string[] args, string profileName, string version)
        {
            return GetArg(args, "-outputPath") ?? Path.Combine("Builds", profileName, version);
        }
    }
}
