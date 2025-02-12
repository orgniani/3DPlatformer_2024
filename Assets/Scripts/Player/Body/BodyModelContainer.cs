using UnityEngine;

namespace Player.Body
{
    [CreateAssetMenu(menuName = "Models/Player/Body", fileName = "PBM_")]
    public class BodyModelContainer : ScriptableObject
    {
        [field: SerializeField] public BodyModel Model { get; private set; }
    }

}
