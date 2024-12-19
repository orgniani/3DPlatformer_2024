using UnityEngine;

namespace Player.Body
{
    [CreateAssetMenu(menuName = "Models/PlayerBody", fileName = "PBM_")]
    public class BodyModelContainer : ScriptableObject
    {
        [field: SerializeField] public BodyModel Model { get; private set; }
    }

}
