using UnityEngine;

namespace Camera.FollowTarget
{
    public class FollowPlayer : MonoBehaviour
    {
        [SerializeField] private Transform target;

        public FollowPlayerModel Model { get; set; }

        private void Awake()
        {
            if (!target)
            {
                Debug.LogError($"{name}: {nameof(target)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            //TODO: Move to a game manager instead
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

}
