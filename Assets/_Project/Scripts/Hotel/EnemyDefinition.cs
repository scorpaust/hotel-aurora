#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Hotel
{
    [CreateAssetMenu(menuName = "Aurora/Content/Enemy Definition", fileName = "SO_NewEnemy")]
    public sealed class EnemyDefinition : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private string _displayName = string.Empty;
        [SerializeField] private RuntimeAnimatorController? _animatorController;
        [SerializeField] private Sprite? _previewSprite;
        [SerializeField] private EnemyDifficulty _difficulty = EnemyDifficulty.Medium;

        [Tooltip("Visual prop the lantern reveals as this enemy's human identity. Never a combat weapon.")]
        [SerializeField] private Sprite? _identityProp;

        [Header("Behaviour (data only — Fase 3 wires the actual AI)")]
        [SerializeField] private float _moveSpeed = 1.5f;
        [SerializeField] private float _detectionRadius = 3f;

        public string Id => _id;
        public string DisplayName => _displayName;
        public RuntimeAnimatorController? AnimatorController => _animatorController;
        public Sprite? PreviewSprite => _previewSprite;
        public EnemyDifficulty Difficulty => _difficulty;
        public Sprite? IdentityProp => _identityProp;
        public float MoveSpeed => _moveSpeed;
        public float DetectionRadius => _detectionRadius;
    }
}
