using Aurora.Core;
using NUnit.Framework;

namespace Aurora.Tests.EditMode
{
    public class LanternHeatTests
    {
        [Test]
        public void Tick_NotFocused_StaysAtZero()
        {
            var heat = new LanternHeat(heatPerSecond: 1f, coolPerSecond: 1f, lockoutSeconds: 3f);

            heat.Tick(focusHeld: false, deltaTime: 1f);

            Assert.AreEqual(0f, heat.Heat01);
            Assert.IsFalse(heat.IsOverheated);
        }

        [Test]
        public void Tick_FocusedBelowThreshold_AccumulatesWithoutOverheating()
        {
            var heat = new LanternHeat(heatPerSecond: 1f / 6f, coolPerSecond: 1f / 4f, lockoutSeconds: 3f);

            heat.Tick(focusHeld: true, deltaTime: 3f); // half of 6s

            Assert.AreEqual(0.5f, heat.Heat01, 0.001f);
            Assert.IsFalse(heat.IsOverheated);
        }

        [Test]
        public void Tick_FocusedPastThreshold_TriggersLockout()
        {
            var heat = new LanternHeat(heatPerSecond: 1f / 6f, coolPerSecond: 1f / 4f, lockoutSeconds: 3f);

            heat.Tick(focusHeld: true, deltaTime: 6f);

            Assert.IsTrue(heat.IsOverheated);
            Assert.AreEqual(3f, heat.LockoutRemaining, 0.001f);
        }

        [Test]
        public void Tick_DuringLockout_IgnoresFocusInput()
        {
            var heat = new LanternHeat(heatPerSecond: 1f / 6f, coolPerSecond: 1f / 4f, lockoutSeconds: 3f);
            heat.Tick(focusHeld: true, deltaTime: 6f); // triggers lockout

            heat.Tick(focusHeld: true, deltaTime: 1f);

            Assert.IsTrue(heat.IsOverheated);
            Assert.AreEqual(2f, heat.LockoutRemaining, 0.001f);
        }

        [Test]
        public void Tick_LockoutExpires_RecoversToUsable()
        {
            var heat = new LanternHeat(heatPerSecond: 1f / 6f, coolPerSecond: 1f / 4f, lockoutSeconds: 3f);
            heat.Tick(focusHeld: true, deltaTime: 6f); // triggers lockout

            heat.Tick(focusHeld: false, deltaTime: 3f); // exactly the lockout duration

            Assert.IsFalse(heat.IsOverheated);
        }

        [Test]
        public void Tick_NotFocusedAfterHeating_CoolsDown()
        {
            var heat = new LanternHeat(heatPerSecond: 1f / 6f, coolPerSecond: 1f / 4f, lockoutSeconds: 3f);
            heat.Tick(focusHeld: true, deltaTime: 3f); // heat to 0.5

            heat.Tick(focusHeld: false, deltaTime: 1f); // cool for 1s at 0.25/s

            Assert.AreEqual(0.25f, heat.Heat01, 0.001f);
        }
    }
}
