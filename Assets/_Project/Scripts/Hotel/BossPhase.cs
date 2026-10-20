#nullable enable

using UnityEngine;

namespace Aurora.Hotel
{
    [System.Serializable]
    public sealed class BossPhase
    {
        [SerializeField] private string _name = string.Empty;

        [Tooltip("What the lantern needs to reveal/do in this phase to progress.")]
        [SerializeField] private string _lanternPuzzleDescription = string.Empty;

        [Tooltip("Free-text condition for now (e.g. a WorldFlags key) — formalised when Fase 5/6 content is built.")]
        [SerializeField] private string _transitionCondition = string.Empty;

        public string Name => _name;
        public string LanternPuzzleDescription => _lanternPuzzleDescription;
        public string TransitionCondition => _transitionCondition;
    }
}
