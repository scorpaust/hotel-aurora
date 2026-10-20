#nullable enable

namespace Aurora.Core
{
    /// <summary>
    /// Set by Aurora.Editor's play-from-anywhere hook before Boot loads; read by GameBootstrap.
    /// </summary>
    public static class BootstrapFlags
    {
        public static bool PlayFromAnywhere { get; set; }
    }
}
