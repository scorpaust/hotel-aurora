#nullable enable

using Aurora.Core;
using UnityEngine;
using UnityEngine.Timeline;

namespace Aurora.Narrative
{
    [CreateAssetMenu(menuName = "Aurora/Content/Cutscene Definition", fileName = "SO_NewCutscene")]
    public sealed class CutsceneDefinition : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private TimelineAsset? _timeline;

        [Tooltip("Free-text trigger condition for now (e.g. a WorldFlags key).")]
        [SerializeField] private string _triggerCondition = string.Empty;

        public string Id => _id;
        public TimelineAsset? Timeline => _timeline;
        public string TriggerCondition => _triggerCondition;
    }
}
