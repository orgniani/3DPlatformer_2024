using UnityEngine;


namespace Camera.FollowTarget
{
    [CreateAssetMenu(menuName = "Models/Camera/FollowPlayerModel", fileName = "CFPM_")]
    public class FollowPlayerModelContainer : ScriptableObject
    {
        [field: SerializeField] public FollowPlayerModel Model { get; private set; }
    }
}