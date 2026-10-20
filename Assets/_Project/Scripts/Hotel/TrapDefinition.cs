#nullable enable

using Aurora.Audio;
using Aurora.Core;
using UnityEngine;

namespace Aurora.Hotel
{
    [CreateAssetMenu(menuName = "Aurora/Content/Trap Definition", fileName = "SO_NewTrap")]
    public sealed class TrapDefinition : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private string _displayName = string.Empty;
        [SerializeField] private TrapType _type = TrapType.Mechanical;

        [Tooltip("Free-text trigger for now (e.g. 'player steps on tile') — formalised with the real trap logic.")]
        [SerializeField] private string _trigger = string.Empty;

        [SerializeField] private TrapIntensity _intensity = TrapIntensity.Medium;
        [SerializeField] private VfxCueDefinition? _vfxCue;
        [SerializeField] private SfxCueDefinition? _sfxCue;
        [SerializeField] private float _cooldownSeconds = 2f;

        public string Id => _id;
        public string DisplayName => _displayName;
        public TrapType Type => _type;
        public string Trigger => _trigger;
        public TrapIntensity Intensity => _intensity;
        public VfxCueDefinition? VfxCue => _vfxCue;
        public SfxCueDefinition? SfxCue => _sfxCue;
        public float CooldownSeconds => _cooldownSeconds;
    }
}
