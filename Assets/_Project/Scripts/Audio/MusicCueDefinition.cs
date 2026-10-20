#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Audio
{
    [CreateAssetMenu(menuName = "Aurora/Content/Music Cue", fileName = "SO_NewMusicCue")]
    public sealed class MusicCueDefinition : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private AudioClip? _clip;
        [SerializeField, Range(0f, 1f)] private float _volume = 1f;
        [SerializeField] private bool _loop = true;
        [SerializeField] private int _priority = 128;

        public string Id => _id;
        public AudioClip? Clip => _clip;
        public float Volume => _volume;
        public bool Loop => _loop;
        public int Priority => _priority;
    }
}
