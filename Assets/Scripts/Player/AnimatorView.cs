using UnityEngine;
using Player.Jump;
using Player.Body;

namespace Player
{
    public class AnimatorView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rigidBody;

        [SerializeField] private PlayerJump jump;
        [SerializeField] private PlayerBody body;

        [Header("Animator Parameters")]
        [SerializeField] private string jumpTriggerParameter = "jump";
        [SerializeField] private string isOnLandParameter = "on_land";
        [SerializeField] private string horSpeedParameter = "hor_speed";

        private void Awake()
        {
            ValidateReferences();
        }

        private void OnEnable()
        {
            jump.OnJump += HandleJump;
        }

        private void OnDisable()
        {
            jump.OnJump -= HandleJump;
        }

        private void Update()
        {
            var velocity = rigidBody.velocity;
            velocity.y = 0;
            var speed = velocity.magnitude;

            animator.SetFloat(horSpeedParameter, speed);
            animator.SetBool(isOnLandParameter, body.IsOnLand);
        }

        private void HandleJump()
        {
            animator.SetTrigger(jumpTriggerParameter);
        }

        private void ValidateReferences()
        {
            if (!animator)
            {
                Debug.LogError($"{name}: {nameof(animator)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }

            if (!rigidBody)
            {
                Debug.LogError($"{name}: {nameof(rigidBody)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }

            if (!jump)
            {
                Debug.LogError($"{name}: {nameof(jump)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }

            if (!body)
            {
                Debug.LogError($"{name}: {nameof(body)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}