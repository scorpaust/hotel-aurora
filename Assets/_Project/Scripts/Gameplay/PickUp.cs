#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Gameplay
{
    /// <summary>
    /// Adds an item to the inventory (IInventoryService, empty until F1-05) and records a
    /// WorldFlag so it doesn't come back — F1-03 spec item 6. Services not yet registered
    /// are simply skipped (Services.TryGet returns false), matching the rest of the project.
    /// </summary>
    public sealed class PickUp : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _verbKey = "interact.pickup";
        [SerializeField] private string _itemId = string.Empty;
        [SerializeField] private string _flagKey = string.Empty;
        [SerializeField] private int _priority;

        public string VerbKey => _verbKey;
        public InteractionKind Kind => InteractionKind.Press;
        public float HoldDuration => 0f;
        public int Priority => _priority;

        public bool CanInteract(InteractionContext context) => true;

        public void Interact(InteractionContext context)
        {
            if (Services.TryGet<IInventoryService>(out var inventory))
                inventory.AddItem(_itemId);

            if (!string.IsNullOrEmpty(_flagKey) && Services.TryGet<IWorldFlagsService>(out var flags))
                flags.SetFlag(_flagKey, 1);

            gameObject.SetActive(false);
        }
    }
}
