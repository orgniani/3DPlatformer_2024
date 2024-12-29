using UnityEngine;
using Events;
using Core;

namespace Gameplay
{
    public class EndOfLevelManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Layers")]
        [SerializeField] private LayerMask playerLayer;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

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
            if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
            {
                if (EventManager<string>.Instance)
                    EventManager<string>.Instance.InvokeEvent(GameEvents.WinAction, true);

                if (enableLogs) Debug.Log($"{name}: <color=orange> Player touched the flag! </color>");
            }
        }
    }
}

