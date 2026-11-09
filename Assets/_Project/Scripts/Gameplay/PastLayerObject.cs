#nullable enable

using Aurora.Core;
using UnityEngine;

namespace Aurora.Gameplay
{
    /// <summary>
    /// An object with a hidden "past" layer, revealed by the lantern's focus beam (F1-04
    /// spec item 4). The SpriteRenderer must already use a material based on the real
    /// SG_RevealPast/M_RevealPast shader (authored in the Editor, not generated here). Each
    /// instance clones that material and drives it directly, every frame, from the active
    /// LanternController's Cone/IsRevealing — a Shader Graph material's own exposed property
    /// value shadows Shader.SetGlobalX for that same property, so a single global broadcast
    /// from the lantern never actually reaches any material; it has to be set per-instance.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class PastLayerObject : MonoBehaviour
    {
        [SerializeField] private RevealMode _mode = RevealMode.Swap;
        [SerializeField] private Texture2D? _presentTexture;
        [SerializeField] private Texture2D? _pastTexture;
        [SerializeField] private Color _pastTint = new(0.75f, 0.9f, 1f, 1f);

        [Header("Physical reveal (optional)")]
        [SerializeField] private bool _physicalWhenRevealed;
        [SerializeField] private Collider2D? _physicalCollider;
        [SerializeField] private float _graceSeconds = 0.5f;

        [Header("Permanent reveal (optional)")]
        [Tooltip("0 = never becomes permanent; the player must keep it lit every time.")]
        [SerializeField] private float _secondsToBecomePermanent;
        [SerializeField] private string _permanentFlagKey = string.Empty;

        private static readonly int ConeOriginId = Shader.PropertyToID("_ConeOrigin");
        private static readonly int ConeDirectionId = Shader.PropertyToID("_ConeDirection");
        private static readonly int ConeHalfAngleId = Shader.PropertyToID("_ConeHalfAngle");
        private static readonly int ConeRangeId = Shader.PropertyToID("_ConeRange");
        private static readonly int EdgeSoftnessId = Shader.PropertyToID("_EdgeSoftness");
        private static readonly int FocusId = Shader.PropertyToID("_Focus");

        private SpriteRenderer _renderer = null!;
        private Material _materialInstance = null!;
        private LanternController? _lanternCache;
        private float _revealedSecondsAccumulated;
        private float _graceRemaining;
        private bool _isPermanentlyRevealed;

        /// <summary>Authoring intent only — the shader blends by texture content, not by this value.</summary>
        public RevealMode Mode => _mode;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _materialInstance = new Material(_renderer.sharedMaterial);
            _renderer.material = _materialInstance;

            if (_presentTexture != null)
                _materialInstance.SetTexture("_PresentTex", _presentTexture);
            if (_pastTexture != null)
                _materialInstance.SetTexture("_PastTex", _pastTexture);
            _materialInstance.SetColor("_PastTint", _pastTint);

            if (!string.IsNullOrEmpty(_permanentFlagKey) && Services.TryGet<IWorldFlagsService>(out var flags) &&
                flags.HasFlag(_permanentFlagKey))
                _isPermanentlyRevealed = true;

            if (_physicalCollider != null)
                _physicalCollider.enabled = _isPermanentlyRevealed;
        }

        private void Update()
        {
            var lantern = ResolveLantern();
            var isLit = _isPermanentlyRevealed || IsCurrentlyLit(lantern);

            PushShaderCone(lantern, isLit);

            if (_physicalWhenRevealed && _physicalCollider != null)
                TickPhysicalCollider(isLit);

            TickPermanentReveal(isLit);
        }

        private void PushShaderCone(LanternController? lantern, bool isLit)
        {
            // A permanent reveal must show "past" everywhere on the object regardless of
            // where the lantern currently points — feed the shader a cone wide enough to
            // always contain it, rather than relying on whatever the real cone is doing.
            if (_isPermanentlyRevealed)
            {
                _materialInstance.SetVector(ConeOriginId, transform.position);
                _materialInstance.SetVector(ConeDirectionId, Vector2.right);
                _materialInstance.SetFloat(ConeHalfAngleId, 180f);
                _materialInstance.SetFloat(ConeRangeId, float.MaxValue);
                _materialInstance.SetFloat(FocusId, 1f);
                return;
            }

            if (lantern == null)
            {
                _materialInstance.SetFloat(FocusId, 0f);
                return;
            }

            var cone = lantern.Cone;
            _materialInstance.SetVector(ConeOriginId, cone.Origin);
            _materialInstance.SetVector(ConeDirectionId, cone.Direction);
            _materialInstance.SetFloat(ConeHalfAngleId, cone.HalfAngleDegrees);
            _materialInstance.SetFloat(ConeRangeId, cone.Range);
            _materialInstance.SetFloat(EdgeSoftnessId, lantern.EdgeSoftness);
            _materialInstance.SetFloat(FocusId, isLit ? 1f : 0f);
        }

        private void TickPhysicalCollider(bool isLit)
        {
            if (isLit)
            {
                _physicalCollider!.enabled = true;
                _graceRemaining = _graceSeconds;
                return;
            }

            if (_graceRemaining > 0f)
            {
                _graceRemaining -= Time.deltaTime;
                _physicalCollider!.enabled = true;
                return;
            }

            _physicalCollider!.enabled = _isPermanentlyRevealed;
        }

        private void TickPermanentReveal(bool isLit)
        {
            if (_isPermanentlyRevealed || _secondsToBecomePermanent <= 0f || string.IsNullOrEmpty(_permanentFlagKey))
                return;

            if (!isLit)
            {
                _revealedSecondsAccumulated = 0f;
                return;
            }

            _revealedSecondsAccumulated += Time.deltaTime;
            if (_revealedSecondsAccumulated < _secondsToBecomePermanent)
                return;

            _isPermanentlyRevealed = true;
            if (Services.TryGet<IWorldFlagsService>(out var flags))
                flags.SetFlag(_permanentFlagKey, 1);
        }

        private LanternController? ResolveLantern()
        {
            if (_lanternCache == null && PlayerController.Current != null)
                _lanternCache = PlayerController.Current.GetComponent<LanternController>();

            return _lanternCache;
        }

        private bool IsCurrentlyLit(LanternController? lantern)
        {
            return lantern != null && lantern.IsRevealing && lantern.Cone.Contains(transform.position);
        }
    }
}
