using Camera.FollowTarget;
using DataSources;
using UnityEngine;

namespace Camera
{
    [RequireComponent(typeof(FollowPlayer))]
    public class CameraSetup : MonoBehaviour
    {
        [Header("Follow Player")]
        [SerializeField] private DataSource<CameraSetup> cameraDataSource;

        [SerializeField] private FollowPlayerModelContainer followPlayerModelContainer;
        private FollowPlayer _followPlayer;

        public FollowPlayerModelContainer FollowPlayerModelContainer
        {
            get
            {
                return followPlayerModelContainer;
            }

            set
            {
                followPlayerModelContainer = value;
                _followPlayer.Model = followPlayerModelContainer.Model;
            }
        }


        private void Awake()
        {
            _followPlayer = GetComponent<FollowPlayer>();
            ValidateReferences();
        }

        private void OnEnable()
        {
            cameraDataSource.Value = this;
        }

        private void OnDisable()
        {
            if (cameraDataSource.Value == this)
                cameraDataSource.Value = null;
        }

        public void SetUp(Transform target)
        {
            if (_followPlayer && followPlayerModelContainer)
            {
                _followPlayer.Model = followPlayerModelContainer.Model;
                _followPlayer.enabled = true;

                _followPlayer.StartFollowingTarget(target);
            }
        }

        public void SetInputRotation(Vector2 input)
        {
            _followPlayer.SetInputRotation(input);
        }


        private void ValidateReferences()
        {
            if (!cameraDataSource)
            {
                Debug.LogError($"{name}: {nameof(cameraDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (followPlayerModelContainer)
            {
                _followPlayer.Model = followPlayerModelContainer.Model;
                _followPlayer.enabled = true;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(followPlayerModelContainer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}
