using Camera.FollowTarget;
using UnityEngine;

namespace Camera
{
    public class CameraSetup : MonoBehaviour
    {
        [Header("Follow Player")]
        [SerializeField] private FollowPlayerModelContainer followPlayerModelContainer;
        [SerializeField] private FollowPlayer followPlayer;

        private void OnEnable()
        {
            if (followPlayer && followPlayerModelContainer)
            {
                followPlayer.Model = followPlayerModelContainer.Model;
                followPlayer.enabled = true;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(followPlayer)} or {nameof(followPlayerModelContainer)} is null!" +
                   $"\nDisabling component to avoid errors.");
            }
        }
    }

}
