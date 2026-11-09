#nullable enable

using System.Collections.Generic;
using Aurora.Core;
using Aurora.Gameplay;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aurora.UI
{
    /// <summary>
    /// HUD prompt following PlayerInteractor's current target in screen space (F1-03 spec
    /// item 4). VerbKey is shown as raw text — there's no ILocalizationService yet (F2-04);
    /// the button icon is a text placeholder until F4-02, as the spec itself allows.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public sealed class InteractionPromptPanel : MonoBehaviour
    {
        [SerializeField] private PlayerInteractor _interactor = null!;

        private VisualElement _root = null!;
        private Camera? _camera;
        private VisualElement _panel = null!;
        private Label _verbLabel = null!;
        private VisualElement _holdTrack = null!;
        private VisualElement _holdFill = null!;

        private void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _root.style.flexGrow = 1;
            _root.pickingMode = PickingMode.Ignore;

            _panel = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    backgroundColor = new Color(0.106f, 0.09f, 0.078f, 0.85f), // --aurora-ink
                    paddingLeft = 8,
                    paddingRight = 8,
                    paddingTop = 4,
                    paddingBottom = 4,
                    opacity = 0f,
                    transitionDuration = new List<TimeValue> { new(0.15f) }
                },
                pickingMode = PickingMode.Ignore
            };

            _verbLabel = new Label
            {
                style = { color = new Color(1f, 0.89f, 0.64f), fontSize = 14, unityTextAlign = TextAnchor.MiddleLeft } // --aurora-light
            };
            _panel.Add(_verbLabel);

            _holdFill = new VisualElement
            {
                style =
                {
                    height = Length.Percent(100),
                    width = Length.Percent(0),
                    backgroundColor = new Color(0.722f, 0.541f, 0.243f) // --aurora-brass
                }
            };
            _holdTrack = new VisualElement
            {
                style =
                {
                    width = 40,
                    height = 4,
                    marginLeft = 6,
                    backgroundColor = new Color(0.478f, 0.361f, 0.165f), // --aurora-brass-dim
                    display = DisplayStyle.None
                }
            };
            _holdTrack.Add(_holdFill);
            _panel.Add(_holdTrack);

            _root.Add(_panel);
        }

        private void Update()
        {
            var target = _interactor.CurrentTarget;
            var camera = ResolveCamera();
            // "as Component" + "!= null" (Unity's overloaded null check) rather than an "is"
            // pattern match, which would still read true for an object Destroy()'d this same
            // frame (the C# wrapper survives one frame; accessing .transform on it throws).
            var component = target as Component;
            var visible = target != null && camera != null && component != null && IsPromptAllowed();

            _panel.style.opacity = visible ? 1f : 0f;
            if (!visible)
                return;

            _verbLabel.text = target!.VerbKey;

            var isHold = target.Kind == InteractionKind.Hold;
            _holdTrack.style.display = isHold ? DisplayStyle.Flex : DisplayStyle.None;
            if (isHold)
                _holdFill.style.width = Length.Percent(_interactor.HoldProgress01 * 100f);

            // Unity's own documented pattern for anchoring UI Toolkit runtime elements to a
            // world Transform: viewport point -> container pixels, Y flipped for UI's
            // top-left origin, applied via style.translate (not left/top, to avoid layout
            // reflow every frame).
            var containerSize = _root.layout.size;
            var viewportPoint = camera!.WorldToViewportPoint(component!.transform.position);
            var x = viewportPoint.x * containerSize.x;
            var y = (1f - viewportPoint.y) * containerSize.y;
            _panel.style.translate = new Translate(x, y);
        }

        private Camera? ResolveCamera()
        {
            _camera = ActiveCameraResolver.Resolve(_camera);
            return _camera;
        }

        private static bool IsPromptAllowed()
        {
            return Services.TryGet<IGameStateService>(out var gameState) &&
                   (gameState.CurrentState == GameState.Playing || gameState.CurrentState == GameState.Chase);
        }
    }
}
