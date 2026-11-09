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
        private Camera? _camera;
        private LanternHeat? _heat;

        private bool _isOn;
        private float _focusBlend;

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

            // Position before aiming: ComputeAimDirection's mouse-to-world conversion reads
            // _pivot.position, so it needs this frame's position, not last frame's.
            UpdatePivotPosition();
            var direction = ComputeAimDirection();
            ApplyLight(direction, overheated);

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
            Vector2 desired;

            if (_input.CurrentDevice == InputDeviceKind.KeyboardMouse)
            {
                _camera = ActiveCameraResolver.Resolve(_camera);
                if (_camera == null)
                    return facing;

                var depth = _camera.WorldToScreenPoint(_pivot.position).z;
                var worldPoint = _camera.ScreenToWorldPoint(new Vector3(raw.x, raw.y, depth));
                desired = (Vector2)worldPoint - (Vector2)_pivot.position;
            }
            else
            {
                desired = raw.magnitude > 0.2f ? raw : facing;
            }

            if (desired.sqrMagnitude < 0.0001f)
                return facing;

            desired.Normalize();

            var signedAngle = Vector2.SignedAngle(facing, desired);
            var clamped = Mathf.Clamp(signedAngle, -_aimClampDegrees, _aimClampDegrees);
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

            // The physical lantern itself (if one is assigned) stays visible and turns with
            // the aim whenever it's on — even overheated, Tomás is still holding it, just not
            // getting light out of it.
            if (_visual != null)
                _visual.enabled = _isOn;

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
    }
}
