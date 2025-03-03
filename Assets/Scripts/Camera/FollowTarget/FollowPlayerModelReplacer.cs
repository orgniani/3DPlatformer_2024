using DataSources;
using UnityEngine;

namespace Camera.FollowTarget
{
    [CreateAssetMenu(menuName = "Models/Camera/FollowPlayerReplacer", fileName = "CFPM_Replacer")]
    public class FollowPlayerModelReplacer : ScriptableObject
    {
        [SerializeField] private FollowPlayerModelContainer controllerFollowPlayerModelContainer;
        [SerializeField] private DataSource<CameraSetup> cameraDataSource;

        private FollowPlayerModelContainer _replacement;
        private CameraSetup _camera;

        private void OnEnable()
        {
            if (!AreReferencesValidated()) return;

            _replacement = controllerFollowPlayerModelContainer;
            TryFindCamera();
        }

        public void ReplaceCameraModelContainer()
        {
            if (!_camera)
            {
                TryFindCamera();
                if (!_camera) return;
            }

            if (_replacement == _camera.FollowPlayerModelContainer)
                _replacement = controllerFollowPlayerModelContainer;

            var temp = _camera.FollowPlayerModelContainer;

            _camera.FollowPlayerModelContainer = _replacement;
            _replacement = temp;
        }

        private void TryFindCamera()
        {
            if (cameraDataSource.Value != null)
                _camera = cameraDataSource.Value;
        }

        private bool AreReferencesValidated()
        {
            if (!controllerFollowPlayerModelContainer)
            {
                Debug.LogError($"{name}: {nameof(controllerFollowPlayerModelContainer)} is null!");
                return false;
            }

            if (!cameraDataSource)
            {
                Debug.LogError($"{name}: {nameof(cameraDataSource)} is null!");
                return false;
            }

            return true;
        }
    }

}