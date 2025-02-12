using UnityEngine;

namespace Player.Jump
{
    [CreateAssetMenu(menuName = "Models/Player/Jump", fileName = "PJM_")]
    public class JumpModelContainer : ScriptableObject
    {
        [field: SerializeField] public JumpModel Model { get; private set; }
    }
}