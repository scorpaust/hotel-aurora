#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Audio
{
    [CreateAssetMenu(menuName = "Aurora/Content/VFX Cue", fileName = "SO_NewVfxCue")]
    public sealed class VfxCueDefinition : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private GameObject? _particlePrefab;
        [SerializeField] private bool _loop;
        [SerializeField] private int _priority = 128;

        public string Id => _id;
        public GameObject? ParticlePrefab => _particlePrefab;
        public bool Loop => _loop;
        public int Priority => _priority;
    }
}
