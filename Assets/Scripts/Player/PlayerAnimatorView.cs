using UnityEngine;
using Player.Jump;
using Player.Body;

namespace Player
{
    //TODO: Perhaps this could be improved --> ANIMATOR VIEW
    public class PlayerAnimatorView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rigidBody;

        [SerializeField] private PlayerJump jump;
        [SerializeField] private PlayerBody body;
        [SerializeField] private PlayerSound sound;

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

        /// <summary>
        /// Triggered by an Animation Event in the player's animation clips that require footstep sound effects.  
        /// This method ensures the event is fired with sufficient weight (above 0.5)  
        /// before delegating the footstep sound handling to the PlayerSound component.
        /// </summary>
        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
                sound.HandleFootstepsAudio();
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

            if (!sound)
            {
                Debug.LogError($"{name}: {nameof(sound)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}