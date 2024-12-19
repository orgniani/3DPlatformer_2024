using UnityEngine;

namespace Player.Rotation
{
    [CreateAssetMenu(menuName = "Models/PlayerRotation", fileName = "PRM_")]
    public class RotationModelContainer : ScriptableObject
    {
        [field: SerializeField] public RotationModel Model { get; private set; }
    }
}
