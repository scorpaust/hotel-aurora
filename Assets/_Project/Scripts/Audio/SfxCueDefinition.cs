#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Audio
{
    [CreateAssetMenu(menuName = "Aurora/Content/SFX Cue", fileName = "SO_NewSfxCue")]
    public sealed class SfxCueDefinition : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private AudioClip? _clip;
        [SerializeField, Range(0f, 1f)] private float _volume = 1f;
        [SerializeField] private bool _loop;
        [SerializeField] private int _priority = 128;

        public string Id => _id;
        public AudioClip? Clip => _clip;
        public float Volume => _volume;
        public bool Loop => _loop;
        public int Priority => _priority;
    }
}
