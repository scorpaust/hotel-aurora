#nullable enable

using Aurora.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Aurora.Gameplay
{
    /// <summary>
    /// The lantern (F1-04): aiming, normal light, focus beam, and overheat. Exposes
    /// Cone/IsRevealing/EdgeSoftness so each PastLayerObject can drive its own
    /// SG_RevealPast/M_RevealPast material instance — a Shader Graph material's own exposed
    /// property value shadows Shader.SetGlobalX for that property, so pushing the cone as
    /// global shader properties here (tried first) never actually reached any material; each
    /// object has to set it on itself. Aiming and heat math live in Aurora.Core
    /// (LanternCone/LanternHeat) so they're testable without a scene.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public sealed class LanternController : MonoBehaviour
    {
        [Header("Rig")]
        [SerializeField] private Transform _pivot = null!;
        [SerializeField] private Light2D _light = null!;
        [SerializeField] private SpriteRenderer? _visual;

        [Header("Aiming")]
        [SerializeField] private float _aimClampDegrees = 70f;
        [SerializeField] private float _lightForwardOffsetDegrees;
        [SerializeField] private Vector2 _handOffset = new(0.14f, 0.18f);
        [Tooltip("Degrees of tilt per pixel of mouse movement (vertical delta).")]
        [SerializeField] private float _mouseSensitivityDegreesPerPixel = 0.25f;

        [Header("Normal light")]
        [SerializeField] private float _normalInnerAngle = 50f;
        [SerializeField] private float _normalOuterAngle = 70f;
        [SerializeField] private float _normalRange = 6f;
        [SerializeField] private float _normalIntensity = 1.2f;
        [SerializeField] private Color _normalColor = Color.white;

        [Header("Focus beam")]
        [SerializeField] private float _focusInnerAngle = 15f;
        [SerializeField] private float _focusOuterAngle = 25f;
        [SerializeField] private float _focusRange = 8f;
        [SerializeField] private float _focusIntensity = 2.2f;
        [SerializeField] private Color _focusColor = new(0.624f, 0.769f, 0.780f); // --aurora-ghost
        [SerializeField] private float _focusLerpSpeed = 5f; // ~0.2s to ramp in/out
        [SerializeField] private float _edgeSoftness = 0.2f;

        [Header("Overheat")]
        [SerializeField] private float _heatPerSecond = 1f / 6f; // ~6s focus to overheat
        [SerializeField] private float _coolPerSecond = 1f / 4f; // ~4s to cool fully
        [SerializeField] private float _overheatLockoutSeconds = 3f;

        private PlayerController _player = null!;
        private IInputService? _input;
        private LanternHeat? _heat;

        private bool _isOn;
        private float _focusBlend;
        private Vector3 _visualBaseScale = Vector3.one;

        /// <summary>
        /// Persistent mouse tilt, in world sense (positive = tilted up) regardless of facing —
        /// moving the mouse up always tilts the beam up, whichever way Tomás faces. Driven by the
        /// mouse's delta control instead of sampling its screen position and converting it
        /// through Camera.ScreenToWorldPoint: that absolute approach repeatedly proved unreliable
        /// in the Editor's Game View (stale/zero readings when the OS cursor isn't tracked as
        /// expected), surviving several rounds of threshold/timeout tuning that never fully fixed
        /// it. A relative delta sidesteps all of that — it never depends on where the OS cursor
        /// physically sits, only on how it moved since last frame.
        /// </summary>
        private float _mouseTiltDegrees;

        /// <summary>True while the past-layer reveal is actually active (on, focused, not overheated).</summary>
        public bool IsRevealing { get; private set; }

        public LanternCone Cone { get; private set; }

        /// <summary>Angular/distance falloff softness (0..1) for PastLayerObject's shader mask.</summary>
        public float EdgeSoftness => _edgeSoftness;

        private void Awake()
        {
            _player = GetComponent<PlayerController>();
            _heat = new LanternHeat(_heatPerSecond, _coolPerSecond, _overheatLockoutSeconds);

            // Detach from Tomás's own transform entirely. His root flips localScale.x to face
            // left (PlayerController.UpdateAnimator) — even with the pivot's own scale
            // counter-acting that (tried first), the Light2D under it still rendered its cone
            // in the wrong direction (confirmed live: aiming left pointed the beam straight
            // down instead). Light2D apparently doesn't resolve direction from decomposed
            // rotation alone once a negative scale has been anywhere in its ancestry, even if
            // the net scale cancels out — so the pivot now has no ancestor to inherit from at
            // all, and Update() positions/rotates it in world space by hand every frame.
            _pivot.SetParent(null, true);

            if (_visual != null)
                _visualBaseScale = _visual.transform.localScale;
        }

        private void Update()
        {
            if (_input == null && !Services.TryGet<IInputService>(out _input))
                return;

            if (_input.Gameplay.Lantern.WasPressedThisFrame())
                _isOn = !_isOn;

            var focusHeld = _isOn && _input.Gameplay.LanternFocus.IsPressed();
            _heat!.Tick(focusHeld, Time.deltaTime);
            var overheated = _heat.IsOverheated;
            var focusActive = focusHeld && !overheated;

            _focusBlend = Mathf.MoveTowards(_focusBlend, focusActive ? 1f : 0f, _focusLerpSpeed * Time.deltaTime);

            // Position before aiming: the Cone built below reads _pivot.position, so it needs
            // this frame's position, not last frame's.
            UpdatePivotPosition();
            var direction = ComputeAimDirection();
            ApplyLight(direction, overheated);
            ApplyVisual();

            IsRevealing = _isOn && !overheated && _focusBlend > 0.01f;

            var halfAngle = Mathf.Lerp(_normalOuterAngle, _focusOuterAngle, _focusBlend) * 0.5f;
            var range = Mathf.Lerp(_normalRange, _focusRange, _focusBlend);
            Cone = new LanternCone(_pivot.position, direction, halfAngle, range);
        }

        private Vector2 ComputeAimDirection()
        {
            var facing = _player.FacingRight ? Vector2.right : Vector2.left;
            if (_input == null)
                return facing;

            var raw = _input.Gameplay.LanternAim.ReadValue<Vector2>();
            float clamped;

            if (_input.CurrentDevice == InputDeviceKind.KeyboardMouse)
            {
                _mouseTiltDegrees = Mathf.Clamp(_mouseTiltDegrees + raw.y * _mouseSensitivityDegreesPerPixel, -_aimClampDegrees, _aimClampDegrees);
                clamped = _player.FacingRight ? _mouseTiltDegrees : -_mouseTiltDegrees;
            }
            else
            {
                var desired = raw.magnitude > 0.2f ? raw : facing;
                if (desired.sqrMagnitude < 0.0001f)
                    return facing;

                desired.Normalize();
                var signedAngle = Vector2.SignedAngle(facing, desired);
                clamped = Mathf.Clamp(signedAngle, -_aimClampDegrees, _aimClampDegrees);
                // Keep the mouse accumulator in sync so switching device mid-aim doesn't jump.
                _mouseTiltDegrees = _player.FacingRight ? clamped : -clamped;
            }

            return RotateDegrees(facing, clamped);
        }

        private static Vector2 RotateDegrees(Vector2 v, float degrees)
        {
            var rad = degrees * Mathf.Deg2Rad;
            var cos = Mathf.Cos(rad);
            var sin = Mathf.Sin(rad);
            return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
        }

        /// <summary>
        /// Unparented (see Awake) — position by hand each frame, mirroring the hand offset to
        /// the other side when facing left, same as parenting would have done for position
        /// (just not for rotation, which is the part that broke).
        /// </summary>
        private void UpdatePivotPosition()
        {
            var offsetX = _player.FacingRight ? _handOffset.x : -_handOffset.x;
            _pivot.position = (Vector2)transform.position + new Vector2(offsetX, _handOffset.y);
        }

        private void ApplyLight(Vector2 direction, bool overheated)
        {
            // Light2D's own "0 rotation" reference points local +Y (up), not +X (right) as
            // Atan2 assumes — confirmed live by forcing known Z values and observing the
            // actual cone direction (0->up, 90->left, 180->down, 270->right). The -90 below
            // converts Atan2's standard +X-relative angle into Light2D's +Y-relative one. This
            // was the real cause of the reported "aiming left points the beam down" bug — not
            // the unparenting above, which turned out not to matter either way.
            var angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            _pivot.rotation = Quaternion.Euler(0f, 0f, angleDeg + _lightForwardOffsetDegrees);

            if (!_isOn)
            {
                _light.enabled = false;
                return;
            }

            if (overheated)
            {
                // Placeholder "dying flashlight" flicker — communicates the overheat lockout
                // without a UI bar (spec item 5), sound comes later in F4-03.
                _light.enabled = true;
                _light.color = _normalColor;
                _light.pointLightInnerAngle = _normalInnerAngle;
                _light.pointLightOuterAngle = _normalOuterAngle;
                _light.pointLightOuterRadius = _normalRange * 0.5f;
                _light.intensity = Mathf.PerlinNoise(Time.time * 15f, 0f) * 0.3f;
                return;
            }

            _light.enabled = true;
            _light.pointLightInnerAngle = Mathf.Lerp(_normalInnerAngle, _focusInnerAngle, _focusBlend);
            _light.pointLightOuterAngle = Mathf.Lerp(_normalOuterAngle, _focusOuterAngle, _focusBlend);
            _light.pointLightOuterRadius = Mathf.Lerp(_normalRange, _focusRange, _focusBlend);
            _light.intensity = Mathf.Lerp(_normalIntensity, _focusIntensity, _focusBlend);
            _light.color = Color.Lerp(_normalColor, _focusColor, _focusBlend);
        }

        /// <summary>
        /// Matches the Light2D's own world rotation (_pivot.rotation, already set this frame by
        /// ApplyLight, which runs first) exactly, instead of re-deriving an angle of its own.
        /// An earlier version computed the visual's rotation from a facing-agnostic reference
        /// vector (always "as if facing right") plus a localScale.x flip, specifically to avoid
        /// an off-center sprite pivot swinging the rendered content to a different vertical
        /// offset on each side. That reference vector pointed along local +Y, which a
        /// localScale.x flip can never affect — so while it fixed the vertical case (reportedly
        /// lower when facing right), it left the visual pointing the same world-horizontal
        /// direction regardless of facing once genuinely aimed up/down, i.e. mirrored on the
        /// wrong axis (reported: the lantern angled opposite the mouse/focus when facing left).
        /// Now that the sprite's own pivot is recentered on its visual content (see codespec.md
        /// §5.9), that original asymmetry is gone at the source, so the visual can safely reuse
        /// the same rotation as the light — which is already correctly mirrored, since `direction`
        /// itself differs in sign between facing sides.
        /// </summary>
        private void ApplyVisual()
        {
            if (_visual == null)
                return;

            _visual.enabled = _isOn;

            // Pinned explicitly rather than left to inherit from _pivot: harmless today (the
            // sprite's own local offset is zero), but keeps the prop's anchor immune to any
            // local offset a future authoring pass leaves on it.
            _visual.transform.position = _pivot.position;
            _visual.transform.rotation = _pivot.rotation;

            // Mirrors any left-right-asymmetric art detail (a handle drawn to one side, say) so
            // it reads as the same lantern held in the other hand, not a different shape.
            var scale = _visualBaseScale;
            scale.x = Mathf.Abs(scale.x) * (_player.FacingRight ? 1f : -1f);
            _visual.transform.localScale = scale;
        }
    }
}
