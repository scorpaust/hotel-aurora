#nullable enable

using System;
using System.Collections.Generic;
using Aurora.Narrative;
using UnityEngine;

namespace Aurora.Hotel
{
    [Serializable]
    public sealed class LevelBeat
    {
        [SerializeField] private LevelBeatType _type;

        [Tooltip("Free-text label for the Editor tooling (e.g. a room/beat name). Never shown to the player.")]
        [SerializeField] private string _label = string.Empty;

        [Header("Payload — only the fields matching Type are used")]
        [SerializeField] private EnemyDefinition[] _enemies = Array.Empty<EnemyDefinition>();
        [SerializeField] private TrapDefinition[] _traps = Array.Empty<TrapDefinition>();
        [SerializeField] private CutsceneDefinition? _cutscene;
        [SerializeField] private BossDefinition? _boss;
        [SerializeField] private NarrativeBeatDefinition? _narrativeBeat;

        public LevelBeatType Type => _type;
        public string Label => _label;
        public IReadOnlyList<EnemyDefinition> Enemies => _enemies;
        public IReadOnlyList<TrapDefinition> Traps => _traps;
        public CutsceneDefinition? Cutscene => _cutscene;
        public BossDefinition? Boss => _boss;
        public NarrativeBeatDefinition? NarrativeBeat => _narrativeBeat;
    }
}
