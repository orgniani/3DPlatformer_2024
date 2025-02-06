using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Camera
{
    public class MenuCamera : MonoBehaviour
    {
        [Header("Logs")]
        [SerializeField] private bool enableLogs = false;

        private UnityEngine.Camera _overlayCamera;

        private void Awake()
        {
            _overlayCamera = GetComponent<UnityEngine.Camera>();
            AddToMainCameraStack();
        }

        private void AddToMainCameraStack()
        {
            UnityEngine.Camera mainCamera = UnityEngine.Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError($"{name}:Main Camera not found!");
                return;
            }

            var mainCamData = mainCamera.GetComponent<UniversalAdditionalCameraData>();
            if (mainCamData == null)
            {
                Debug.LogError($"{name}:Main Camera does not have UniversalAdditionalCameraData!");
                return;
            }

            if (!mainCamData.cameraStack.Contains(_overlayCamera))
            {
                mainCamData.cameraStack.Add(_overlayCamera);
                if (enableLogs) Debug.Log($"{name}: <color=green> Menu Camera added to Main Camera stack! </color>");
            }
        }
    }
}
