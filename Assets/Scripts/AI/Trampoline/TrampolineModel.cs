using Audio;
using System;
using UnityEngine;

namespace AI.Trampoline
{
    [Serializable]
    public class TrampolineModel
    {
        [field: SerializeField] public float DepressionAmount { get; private set; } = 0.3f;

        [field: SerializeField] public float ElevationAmount { get; private set; } = 1.5f;

        [field: SerializeField] public AnimationCurve BounceCurve { get; private set; }

        [field: SerializeField] public float BounceSpeed { get; private set; } = 8f;

        [field: SerializeField] public float RecoverySpeed { get; private set; } = 2f;

        [field: SerializeField] public float LaunchForce { get; private set; } = 5f;

        [field: SerializeField] public LayerMask PlayerLayer { get; private set; }

        [field: SerializeField] public AudioConfig BounceAudio { get; private set; }
    }
}