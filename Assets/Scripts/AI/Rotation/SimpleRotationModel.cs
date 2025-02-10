using Audio;
using System;
using UnityEngine;

namespace AI.Rotation
{
    [Serializable]
    public class SimpleRotationModel
    {
        [field: SerializeField] public AudioEvent RotationAudio { get; private set; }

        [field: SerializeField] public float Speed { get; private set; } = 1f;

        [field: SerializeField] public Vector3 RotationAxis { get; private set; } = Vector3.up;

        [field: SerializeField] public AnimationCurve RotationCurve { get; private set; }
    }
}