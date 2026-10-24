using Aurora.Gameplay;
using NUnit.Framework;
using UnityEngine;

namespace Aurora.Tests.EditMode
{
    public class PlayerMotorTests
    {
        private PlayerMovementConfig _config = null!;
        private PlayerMotor _motor = null!;

        [SetUp]
        public void SetUp()
        {
            _config = ScriptableObject.CreateInstance<PlayerMovementConfig>();
            _motor = new PlayerMotor(_config);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_config);
        }

        [Test]
        public void Tick_NoInput_StaysIdle()
        {
            _motor.Tick(0f, false, false, true, 1f / 60f);

            Assert.IsFalse(_motor.IsMoving);
            Assert.AreEqual(Vector2.zero, _motor.Velocity);
        }

        [Test]
        public void Tick_WalkInput_AcceleratesTowardWalkSpeed()
        {
            for (var i = 0; i < 60; i++)
                _motor.Tick(1f, false, false, true, 1f / 60f);

            Assert.IsTrue(_motor.IsMoving);
            Assert.AreEqual(_config.WalkSpeed, _motor.Velocity.x, 0.01f);
        }

        [Test]
        public void Tick_RunHeld_ReachesRunSpeedBeforeBreathRunsOut()
        {
            for (var i = 0; i < 60; i++)
                _motor.Tick(1f, true, false, true, 1f / 60f);

            Assert.IsTrue(_motor.IsRunning);
            Assert.AreEqual(_config.RunSpeed, _motor.Velocity.x, 0.01f);
        }

        [Test]
        public void Tick_RunningPastBreathDuration_SlowsDownAutomatically()
        {
            var ticksForBreathDuration = Mathf.CeilToInt(_config.BreathDuration * 60f) + 30;
            for (var i = 0; i < ticksForBreathDuration; i++)
                _motor.Tick(1f, true, false, true, 1f / 60f);

            Assert.IsFalse(_motor.IsRunning);
            Assert.AreEqual(0f, _motor.BreathFraction01, 0.01f);
            Assert.AreEqual(_config.WalkSpeed, _motor.Velocity.x, 0.01f);
        }

        [Test]
        public void Tick_RecoversBreath_WhileWalkingNotRunning()
        {
            var ticksForBreathDuration = Mathf.CeilToInt(_config.BreathDuration * 60f) + 5;
            for (var i = 0; i < ticksForBreathDuration; i++)
                _motor.Tick(1f, true, false, true, 1f / 60f);

            for (var i = 0; i < 60; i++)
                _motor.Tick(1f, false, false, true, 1f / 60f);

            Assert.Greater(_motor.BreathFraction01, 0f);
        }

        [Test]
        public void Tick_CrouchHeld_SetsIsCrouchingAndCrouchSpeed()
        {
            for (var i = 0; i < 60; i++)
                _motor.Tick(1f, false, true, true, 1f / 60f);

            Assert.IsTrue(_motor.IsCrouching);
            Assert.AreEqual(_config.CrouchSpeed, _motor.Velocity.x, 0.01f);
        }

        [Test]
        public void Tick_CannotStand_ForcesCrouchEvenWithoutCrouchInput()
        {
            _motor.Tick(0f, false, false, canStand: false, 1f / 60f);

            Assert.IsTrue(_motor.IsCrouching);
        }

        [Test]
        public void Tick_FacingFollowsLastNonZeroMoveDirection()
        {
            _motor.Tick(-1f, false, false, true, 1f / 60f);
            Assert.IsFalse(_motor.FacingRight);

            _motor.Tick(1f, false, false, true, 1f / 60f);
            Assert.IsTrue(_motor.FacingRight);

            _motor.Tick(0f, false, false, true, 1f / 60f);
            Assert.IsTrue(_motor.FacingRight, "Facing should hold its last direction when input stops.");
        }

        [Test]
        public void Tick_PartialAnalogInput_ScalesSpeedProportionally()
        {
            for (var i = 0; i < 120; i++)
                _motor.Tick(0.5f, false, false, true, 1f / 60f);

            Assert.AreEqual(_config.WalkSpeed * 0.5f, _motor.Velocity.x, 0.01f,
                "An analog stick pushed halfway should walk at half speed, not full speed.");
        }

        [Test]
        public void EnterStairs_SetsIsOnStairsAndClampsInitialProgress()
        {
            _motor.EnterStairs(1.5f);

            Assert.IsTrue(_motor.IsOnStairs);
            Assert.AreEqual(1f, _motor.StairProgress);
        }

        [Test]
        public void TickOnStairs_MovingUp_IncreasesProgressTowardTop()
        {
            _motor.EnterStairs(0f);

            for (var i = 0; i < 60; i++)
                _motor.TickOnStairs(1f, 2f, 1f, 1f / 60f);

            Assert.AreEqual(1f, _motor.StairProgress, 0.001f);
            Assert.IsTrue(_motor.IsMoving);
        }

        [Test]
        public void TickOnStairs_MovingDown_DecreasesProgressTowardBottom()
        {
            _motor.EnterStairs(1f);

            for (var i = 0; i < 60; i++)
                _motor.TickOnStairs(-1f, 2f, 1f, 1f / 60f);

            Assert.AreEqual(0f, _motor.StairProgress, 0.001f);
        }

        [Test]
        public void TickOnStairs_NoInput_DoesNotMoveAndStaysIdle()
        {
            _motor.EnterStairs(0.5f);
            _motor.TickOnStairs(0f, 2f, 1f, 1f / 60f);

            Assert.AreEqual(0.5f, _motor.StairProgress, 0.001f);
            Assert.IsFalse(_motor.IsMoving);
        }

        [Test]
        public void ExitStairs_ClearsIsOnStairs()
        {
            _motor.EnterStairs(0.5f);
            _motor.ExitStairs();

            Assert.IsFalse(_motor.IsOnStairs);
        }
    }
}
