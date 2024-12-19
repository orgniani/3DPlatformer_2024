using System;
using UnityEngine;

namespace Camera.FollowTarget
{
    [Serializable]
    public class FollowPlayerModel
    {
        [field: SerializeField] public float OffsetUp { get; private set; } = 1.5f;

        [field: SerializeField] public float Speed { get; private set; } = 8f;

        [field: SerializeField] public float RotationSpeed { get; private set; } = 8f;

        [field: SerializeField] public float Distance { get; private set; } = 3f;

        [field: SerializeField] public float MinVerticalAngle { get; private set; } = 5f;

        [field: SerializeField] public float MaxVerticalAngle { get; private set; } = 60f;

        [field: SerializeField] public float Sensitivity { get; private set; } = 3f;
    }
}
