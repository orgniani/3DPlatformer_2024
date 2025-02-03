using UnityEngine;

namespace Player.Brain
{
    [CreateAssetMenu(menuName = "Models/PlayerBrain/BrainContainer", fileName = "BRM_")]
    public class BrainModelContainer : ScriptableObject
    {
        [field: SerializeField] public BrainModel Model { get; private set; }
    }
}
