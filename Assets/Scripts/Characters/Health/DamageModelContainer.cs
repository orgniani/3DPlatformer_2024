using UnityEngine;

namespace Characters.Health
{
    [CreateAssetMenu(menuName = "Models/Damage/DamageContainer", fileName = "DM_")]
    public class DamageModelContainer : ScriptableObject
    {
        [field: SerializeField] public DamageModel Model { get; private set; }
    }
}
