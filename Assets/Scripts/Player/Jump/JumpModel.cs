using System;
using UnityEngine;

namespace Player.Jump
{
    [Serializable]
    public class JumpModel
    {
        [field: SerializeField] public float Force { get; private set; } = 5f;

        [field: SerializeField] public float FloorAngle { get; private set; } = 35;

        [field: SerializeField] public float JumpAcceleration { get; private set; } = 8f;

        [field: SerializeField] public float BrakeMultiplier { get; private set; } = 0.5f;

        [field: SerializeField] public float Cooldown { get; private set; } = 1f;

        [field: SerializeField] public float WaitToJump { get; private set; } = 0.3f;
    }
}
