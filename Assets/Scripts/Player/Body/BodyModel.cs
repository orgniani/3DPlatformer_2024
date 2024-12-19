using System;
using UnityEngine;

namespace Player.Body
{
    [Serializable]
    public class BodyModel
    {
        [field: SerializeField] public float MaxFloorDistance { get; private set; } = 0.7f;

        [field: SerializeField] public float LandBrakeMultiplier { get; private set; } = 0.8f;

        [field: SerializeField] public LayerMask FloorMask { get; private set; }

        [field: SerializeField] public float FloorLineCheckOffset { get; private set; } = 0.1f;

        [field: SerializeField] public float FloorSphereCheckOffset { get; private set; } = -0.2f;

        [field: SerializeField] public float FloorSphereCheckRadius { get; private set; } = 0.3f;
    }
}