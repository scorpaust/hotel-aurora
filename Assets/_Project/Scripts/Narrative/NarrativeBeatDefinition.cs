#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Narrative
{
    [CreateAssetMenu(menuName = "Aurora/Content/Narrative Beat Definition", fileName = "SO_NewNarrativeBeat")]
    public sealed class NarrativeBeatDefinition : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private DialogueGraph? _dialogue;
        [SerializeField] private string _captionKey = string.Empty;

        public string Id => _id;
        public DialogueGraph? Dialogue => _dialogue;
        public string CaptionKey => _captionKey;
    }
}
