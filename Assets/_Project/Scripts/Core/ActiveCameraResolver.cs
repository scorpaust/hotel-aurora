#nullable enable

using UnityEngine;

namespace Aurora.Core
{
    /// <summary>
    /// Several cameras can be enabled at once (Boot's own fallback camera, the F1-01 corridor
    /// camera, an F1-02 RoomPlayground camera) — the one actually on screen is whichever has
    /// the highest depth, same rule the renderer itself uses to layer them. Used wherever
    /// screen-space code (UI follow, lantern mouse aim) needs "the" active camera.
    /// </summary>
    public static class ActiveCameraResolver
    {
        public static Camera? Resolve(Camera? cached)
        {
            if (cached != null && cached.isActiveAndEnabled)
                return cached;

            var count = Camera.allCamerasCount;
            if (count == 0)
                return null;

            var buffer = new Camera[count];
            Camera.GetAllCameras(buffer);

            Camera? best = null;
            for (var i = 0; i < count; i++)
            {
                if (best == null || buffer[i].depth > best.depth)
                    best = buffer[i];
            }

            return best;
        }
    }
}
