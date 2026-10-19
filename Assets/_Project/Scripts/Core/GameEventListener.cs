#nullable enable

using UnityEngine;
using UnityEngine.Events;

namespace Aurora.Core
{
    public sealed class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent? _event;
        [SerializeField] private UnityEvent? _response;

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
