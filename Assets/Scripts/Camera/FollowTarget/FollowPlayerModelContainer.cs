using UnityEngine;


namespace Camera.FollowTarget
{
    [CreateAssetMenu(menuName = "Models/CameraFollowPlayer", fileName = "CFPM_")]
    public class FollowPlayerModelContainer : ScriptableObject
    {
        [field: SerializeField] public FollowPlayerModel Model { get; private set; }
    }
}