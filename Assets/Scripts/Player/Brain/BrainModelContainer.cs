using UnityEngine;

namespace Player.Brain
{
    [CreateAssetMenu(menuName = "Models/Player/Brain/BrainContainer", fileName = "BRM_")]
    public class BrainModelContainer : ScriptableObject
    {
        [field: SerializeField] public BrainModel Model { get; private set; }
    }
}
