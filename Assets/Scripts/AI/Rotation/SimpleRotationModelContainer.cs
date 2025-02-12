using UnityEngine;

namespace AI.Rotation
{
    [CreateAssetMenu(menuName = "Models/AI/SimpleRotation", fileName = "SRM_")]
    public class SimpleRotationModelContainer : ScriptableObject
    {
        [field: SerializeField] public SimpleRotationModel Model { get; private set; }
    }
}
