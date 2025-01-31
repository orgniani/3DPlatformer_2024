using UnityEngine;
using Player.Jump;
using Player.Body;
using Audio;
using Events;

namespace Player
{
    //TODO: Perhaps this could be improved --> ANIMATOR VIEW
    public class AnimatorView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rigidBody;

        [SerializeField] private PlayerJump jump;
        [SerializeField] private PlayerBody body;

        //TODO: Should this be moved?
        [SerializeField] private AudioEvent[] footstepsAudioClips;

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

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                //TODO: Make a check in the validate references :) --> FOOTSTEPS
                if (footstepsAudioClips.Length > 0)
                {
                    var index = Random.Range(0, footstepsAudioClips.Length);
                    if (EventManager<string>.Instance)
                        EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, footstepsAudioClips[index], gameObject);
                }
            }
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