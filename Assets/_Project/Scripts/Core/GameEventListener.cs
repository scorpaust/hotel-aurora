#nullable enable

using UnityEngine;
using UnityEngine.Events;

namespace Aurora.Core
{
    public sealed class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent? _event;
        [SerializeField] private UnityEvent? _response;

        /// <summary>Editor-authoring helpers — let tooling wire this listener without SerializedObject.</summary>
        public UnityEvent Response => _response ??= new UnityEvent();
        public void SetEvent(GameEvent gameEvent) => _event = gameEvent;

        private void OnEnable()
        {
            _event?.Register(OnRaised);
        }

        private void OnDisable()
        {
            _event?.Unregister(OnRaised);
        }

        private void OnRaised()
        {
            _response?.Invoke();
        }
    }
}
