#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Hotel
{
    [CreateAssetMenu(menuName = "Aurora/Content/Boss Definition", fileName = "SO_NewBoss")]
    public sealed class BossDefinition : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private EnemyDefinition? _baseEnemy;
        [SerializeField] private BossPhase[] _phases = System.Array.Empty<BossPhase>();
        [SerializeField] private MemoryDefinition? _recoveredMemory;
        [SerializeField] private GuestBookEntry? _guestBookEntry;

        public string Id => _id;
        public EnemyDefinition? BaseEnemy => _baseEnemy;
        public System.Collections.Generic.IReadOnlyList<BossPhase> Phases => _phases;
        public MemoryDefinition? RecoveredMemory => _recoveredMemory;
        public GuestBookEntry? GuestBookEntry => _guestBookEntry;
    }
}
