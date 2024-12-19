using System;
using UnityEngine;

namespace Player.Rotation
{
    [Serializable]
    public class RotationModel
    {
        [field: SerializeField] public float RotationSpeed { get; private set; } = 5f;

        [field: SerializeField] public float MinimumSpeedForRotation { get; private set; } = 0.001f;
    }
}
