using Camera.FollowTarget;
using DataSources;
using UnityEngine;

namespace Camera
{
    public class CameraSetup : MonoBehaviour
    {
        [Header("Follow Player")]
        [SerializeField] private DataSource<CameraSetup> cameraDataSource;

        [SerializeField] private FollowPlayerModelContainer followPlayerModelContainer;
        [SerializeField] private FollowPlayer followPlayer;

        private void OnEnable()
        {
            cameraDataSource.Value = this;

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

        private void OnDisable()
        {
            if (cameraDataSource != null && cameraDataSource.Value == this)
                cameraDataSource.Value = null;
        }

        public void SetUp(Transform target)
        {
            if (followPlayer && followPlayerModelContainer)
            {
                followPlayer.Model = followPlayerModelContainer.Model;
                followPlayer.enabled = true;

                followPlayer.Target = target;
            }
        }

        public void SetInputRotation(Vector2 input)
        {
            followPlayer.SetInputRotation(input);
        }
    }

}
