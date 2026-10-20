#nullable enable

using UnityEngine;

namespace Aurora.Core
{
    /// <summary>
    /// Placeholder until F2-01 (Sistema de diálogo) implements the real node graph.
    /// Exists now only so other content definitions (bosses, narrative beats) have a
    /// stable type to reference.
    /// </summary>
    [CreateAssetMenu(menuName = "Aurora/Content/Dialogue Graph (placeholder)", fileName = "SO_NewDialogueGraph")]
    public sealed class DialogueGraph : ScriptableObject, IContentDefinition
    {
        [SerializeField] private string _id = string.Empty;

        public string Id => _id;
    }
}
