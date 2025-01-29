using UnityEngine;
using Events;
using Audio;

namespace Gameplay
{
    public class EndOfLevelManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Layers")]
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private AudioEvent winAudio;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private bool _shouldCollide = true;

        private void Awake()
        {
            if (playerLayer == 0)
            {
                Debug.LogError($"{name}: {nameof(playerLayer)} is not set!");
                return;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_shouldCollide) return;

            if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
            {
                if (EventManager<string>.Instance)
                {
                    EventManager<string>.Instance.InvokeEvent(GameEvents.WinAction, true);
                    EventManager<string>.Instance.InvokeEvent(GameEvents.AudioAction, winAudio, transform.position);
                }

                if (enableLogs) Debug.Log($"{name}: <color=orange> Player touched the flag! </color>");

                _shouldCollide = false;
            }
        }
    }
}

