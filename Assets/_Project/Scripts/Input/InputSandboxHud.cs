#if AURORA_DEV
using System;
using Aurora.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Aurora.Input
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class InputSandboxHud : MonoBehaviour
    {
        private InputService _service;
        private Label _deviceLabel;
        private Label _actionLabel;
        private Label _moveLabel;

        private void OnEnable()
        {
            _service = new InputService();
            _service.DeviceChanged += OnDeviceChanged;
            InputSystem.onActionChange += OnActionChange;

            BuildUi();
        }

        private void OnDisable()
        {
            InputSystem.onActionChange -= OnActionChange;
            _service.DeviceChanged -= OnDeviceChanged;
            ((IDisposable)_service).Dispose();
        }

        private void Update()
        {
            if (_moveLabel == null)
                return;

            _moveLabel.text = $"Move: {_service.Gameplay.Move.ReadValue<Vector2>()}";
        }

        private void BuildUi()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.style.position = Position.Absolute;
            root.style.top = 16;
            root.style.left = 16;

            var panel = new VisualElement();
            panel.style.backgroundColor = new Color(0f, 0f, 0f, 0.6f);
            panel.style.paddingTop = 8;
            panel.style.paddingBottom = 8;
            panel.style.paddingLeft = 12;
            panel.style.paddingRight = 12;

            _deviceLabel = new Label($"Device: {_service.CurrentDevice}");
            _actionLabel = new Label("Last action: -");
            _moveLabel = new Label("Move: (0, 0)");

            foreach (var label in new[] { _deviceLabel, _actionLabel, _moveLabel })
            {
                label.style.color = Color.white;
                label.style.fontSize = 18;
                panel.Add(label);
            }

            root.Add(panel);
        }

        private void OnDeviceChanged(InputDeviceKind kind)
        {
            _deviceLabel.text = $"Device: {kind}";
        }

        private void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.ActionPerformed || obj is not InputAction action)
                return;

            _actionLabel.text = $"Last action: {action.name}";
        }
    }
}
#endif
