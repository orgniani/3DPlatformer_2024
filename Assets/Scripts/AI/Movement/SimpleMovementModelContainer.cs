using UnityEngine;

namespace AI.Movement
{
    [CreateAssetMenu(menuName = "Models/AI/SimpleMovement", fileName = "SMM_")]
    public class SimpleMovementModelContainer : ScriptableObject
    {
        [field: SerializeField] public SimpleMovementModel Model { get; private set; }
    }
}
