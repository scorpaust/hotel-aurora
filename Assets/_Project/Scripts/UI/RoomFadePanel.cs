#nullable enable

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aurora.UI
{
    /// <summary>
    /// Full-screen black fade for room transitions (F1-02 spec item 3). Wired to the
    /// SO_RoomFadeOutRequested/SO_RoomFadeInRequested GameEvents via GameEventListener in
    /// the Inspector, so Aurora.Hotel never has to reference Aurora.UI directly.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public sealed class RoomFadePanel : MonoBehaviour
    {
        [SerializeField] private float _fadeDuration = 0.25f;

        private VisualElement _panel = null!;

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.style.flexGrow = 1;
            root.pickingMode = PickingMode.Ignore;

            _panel = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    left = 0,
                    right = 0,
                    top = 0,
                    bottom = 0,
                    backgroundColor = Color.black,
                    opacity = 0f,
                    transitionDuration = new List<TimeValue> { new(_fadeDuration) }
                },
                pickingMode = PickingMode.Ignore
            };
            root.Add(_panel);
        }

        public void FadeOut() => _panel.style.opacity = 1f;
        public void FadeIn() => _panel.style.opacity = 0f;
    }
}
