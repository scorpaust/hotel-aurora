#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Gameplay
{
    /// <summary>
    /// Generic lever/button: flips a WorldFlag and optionally raises an event — the
    /// "mecanismos" building block from the F1-03 objective (puzzles, hotel-event triggers).
    /// </summary>
    public sealed class Switch : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _verbKey = "interact.switch";
        [SerializeField] private string _flagKey = string.Empty;
        [SerializeField] private int _onValue = 1;
        [SerializeField] private int _offValue;
        [SerializeField] private bool _toggles = true;
        [SerializeField] private int _priority;
        [SerializeField] private GameEvent? _onToggled;

        // Some switches (e.g. a jammed lever) are held rather than pressed — spec item 3.
        [SerializeField] private InteractionKind _kind = InteractionKind.Press;
        [SerializeField] private float _holdDuration = 1f;

        public string VerbKey => _verbKey;
        public InteractionKind Kind => _kind;
        public float HoldDuration => _kind == InteractionKind.Hold ? _holdDuration : 0f;
        public int Priority => _priority;

        public bool CanInteract(InteractionContext context) => true;

        public void Interact(InteractionContext context)
        {
            if (string.IsNullOrEmpty(_flagKey) || !Services.TryGet<IWorldFlagsService>(out var flags))
                return;

            var isOn = flags.GetFlag(_flagKey, _offValue) == _onValue;
            var next = _toggles && isOn ? _offValue : _onValue;
            flags.SetFlag(_flagKey, next);

            _onToggled?.Raise();
        }
    }
}
