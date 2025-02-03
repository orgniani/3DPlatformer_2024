using System;
using UnityEngine;

namespace Characters.Health
{
    [Serializable]
    public class DamageModel
    {
        [field: SerializeField] public bool IsInvincible { get; private set; } = false;
    }
}