using UnityEngine;

namespace Aurora.Core
{
    /// <summary>
    /// Pure overheat timer for the lantern's focus beam (F1-04 spec item 5) — heats up while
    /// focus is held, cools down otherwise, locks out for a fixed duration once it hits 1.
    /// </summary>
    public sealed class LanternHeat
    {
        private readonly float _heatPerSecond;
        private readonly float _coolPerSecond;
        private readonly float _lockoutSeconds;

        public float Heat01 { get; private set; }
        public float LockoutRemaining { get; private set; }
        public bool IsOverheated => LockoutRemaining > 0f;

        public LanternHeat(float heatPerSecond, float coolPerSecond, float lockoutSeconds)
        {
            _heatPerSecond = heatPerSecond;
            _coolPerSecond = coolPerSecond;
            _lockoutSeconds = lockoutSeconds;
        }

        public void Tick(bool focusHeld, float deltaTime)
        {
            if (LockoutRemaining > 0f)
            {
                LockoutRemaining = Mathf.Max(0f, LockoutRemaining - deltaTime);
                return;
            }

            if (focusHeld)
            {
                Heat01 = Mathf.Min(1f, Heat01 + _heatPerSecond * deltaTime);
                if (Heat01 >= 1f)
                    LockoutRemaining = _lockoutSeconds;
            }
            else
            {
                Heat01 = Mathf.Max(0f, Heat01 - _coolPerSecond * deltaTime);
            }
        }
    }
}
