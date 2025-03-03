using Audio;
using Events;
using Player.Jump;
using UnityEngine;

namespace Player
{
    public class PlayerSound : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private PlayerJump jump;

        [Header("Audio Configs")]
        [SerializeField] private AudioConfig[] footstepsAudio;
        [SerializeField] private AudioConfig jumpAudio;

        private void Awake()
        {
            ValidateReferences();
        }

        private void OnEnable()
        {
            jump.OnJump += HandleJumpAudio;
        }

        private void OnDisable()
        {
            jump.OnJump -= HandleJumpAudio;
        }

        private void HandleJumpAudio()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, jumpAudio, gameObject);
        }

        public void HandleFootstepsAudio()
        {
            var index = Random.Range(0, footstepsAudio.Length);
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, footstepsAudio[index], gameObject);
        }

        private void ValidateReferences()
        {
            if (!jump)
            {
                Debug.LogError($"{name}: {nameof(jump)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }

            if (footstepsAudio.Length == 0) Debug.LogError($"{name}: {nameof(footstepsAudio)} array is empty!");

            if (!jumpAudio) Debug.LogError($"{name}: {nameof(jumpAudio)} is null!");
        }
    }
}