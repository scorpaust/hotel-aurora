#nullable enable

using UnityEngine;

namespace Aurora.Gameplay
{
    [CreateAssetMenu(menuName = "Aurora/Gameplay/Player Movement Config", fileName = "SO_PlayerMovementConfig")]
    public sealed class PlayerMovementConfig : ScriptableObject
    {
        [Header("Speeds (units/second)")]
        [SerializeField] private float _walkSpeed = 2.2f;
        [SerializeField] private float _runSpeed = 4.5f;
        [SerializeField] private float _crouchSpeed = 1.2f;

        [Header("Acceleration (seconds to reach target speed)")]
        [SerializeField] private float _accelerationTime = 0.12f;
        [SerializeField] private float _decelerationTime = 0.12f;

        [Header("Breath (running stamina)")]
        [SerializeField] private float _breathDuration = 4f;
        [SerializeField] private float _breathRecoveryRate = 1f;

        [Header("Falling")]
        [SerializeField] private float _maxSafeFallDistance = 2f;

        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;
        public float CrouchSpeed => _crouchSpeed;
        public float AccelerationTime => _accelerationTime;
        public float DecelerationTime => _decelerationTime;
        public float BreathDuration => _breathDuration;
        public float BreathRecoveryRate => _breathRecoveryRate;
        public float MaxSafeFallDistance => _maxSafeFallDistance;
    }
}
