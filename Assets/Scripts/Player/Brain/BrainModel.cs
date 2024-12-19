using System;
using UnityEngine;

namespace Player.Brain
{
    [Serializable]
    public class BrainModel
    {
        [field: SerializeField] public float Speed { get; private set; } = 8f;

        [field: SerializeField] public float Acceleration { get; private set; } = 12f;

        [field: SerializeField] public float MovementBreakMultiplier { get; private set; } = 0.2f;
    }
}
