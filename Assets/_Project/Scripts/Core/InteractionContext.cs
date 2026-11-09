#nullable enable

using UnityEngine;

namespace Aurora.Core
{
    /// <summary>What an IInteractable needs to know about who's interacting with it.</summary>
    public readonly struct InteractionContext
    {
        public GameObject Actor { get; }

        public InteractionContext(GameObject actor)
        {
            Actor = actor;
        }
    }
}
