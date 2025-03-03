using UnityEngine;
using Audio;
using Events;

namespace AI
{
    public class LinearPushTrigger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LinearPush[] obstacles;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private AudioConfig triggerAudio;

        private bool _shouldCollide = true;

        private void Awake()
        {
            ValidateReferences();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_shouldCollide) return;

            if (((1 << other.gameObject.layer) & targetLayer.value) != 0)
            {
                if (EventManager<string>.Instance)
                    EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, triggerAudio, gameObject);

                foreach (var obstacle in obstacles)
                    StartCoroutine(obstacle.PushForward());

                _shouldCollide = false;
            }
        }

        private void ValidateReferences()
        {
            if (obstacles.Length <= 0)
            {
                Debug.LogError($"{name}: {nameof(obstacles)} array is empty!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (targetLayer == 0) Debug.LogError($"{name}: {nameof(targetLayer)} is not set!");

            if (!triggerAudio) Debug.LogError($"{name}: {nameof(triggerAudio)} is null!");
        }
    }
}