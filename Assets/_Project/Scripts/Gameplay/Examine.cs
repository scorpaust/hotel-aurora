#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Gameplay
{
    /// <summary>
    /// A short thought Tomás can have about an object (F1-03 spec item 5) — never pauses, so
    /// it just raises an optional event with a localization key; a future narrative/UI
    /// feature decides how to show it (balão de pensamento, uispec §3.4 style).
    /// </summary>
    public sealed class Examine : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _verbKey = "interact.examine";
        [SerializeField] private string _textKey = string.Empty;
        [SerializeField] private int _priority;
        [SerializeField] private GameEventString? _onExamine;

        public string VerbKey => _verbKey;
        public InteractionKind Kind => InteractionKind.Press;
        public float HoldDuration => 0f;
        public int Priority => _priority;

        public bool CanInteract(InteractionContext context) => true;

        public void Interact(InteractionContext context) => _onExamine?.Raise(_textKey);
    }
}
