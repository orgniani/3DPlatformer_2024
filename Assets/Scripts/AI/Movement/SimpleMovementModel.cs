using Audio;
using System;
using UnityEngine;

namespace AI.Movement
{
    [Serializable]
    public class SimpleMovementModel
    {
        [field: SerializeField] public AudioConfig ImpactAudio { get; private set; }

        [field: SerializeField] public float Speed { get; private set; } = 2f;

        [field: SerializeField] public float ImpactThreshold { get; private set; } = 0.95f;

        [field: SerializeField] public AnimationCurve MovementCurve { get; private set; }
    }
}