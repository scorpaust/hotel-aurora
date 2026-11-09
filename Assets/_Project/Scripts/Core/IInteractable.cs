namespace Aurora.Core
{
    /// <summary>
    /// The one way anything in the world responds to Interact (F1-03). Implemented by
    /// MonoBehaviours that PlayerInteractor finds via OverlapCircle and picks by Priority
    /// then distance.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>Localization key for the prompt's verb (e.g. "interact.open").</summary>
        string VerbKey { get; }

        InteractionKind Kind { get; }

        /// <summary>Only meaningful when Kind is Hold.</summary>
        float HoldDuration { get; }

        /// <summary>Higher wins when multiple interactables are in range at once.</summary>
        int Priority { get; }

        bool CanInteract(InteractionContext context);

        void Interact(InteractionContext context);
    }
}
