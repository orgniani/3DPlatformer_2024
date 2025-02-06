using UnityEngine;
using Characters;
using DataSources;
using Audio;
using Events;

namespace AI
{
    public class Obstacle : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<Character> targetDataSource;

        [Header("Layers")]
        [SerializeField] private LayerMask targetLayer;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        [Header("Audio")]
        [SerializeField] private AudioEvent idleAudio;


        //TODO: ITARGET??
        private Character _target;

        private void Awake()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, idleAudio, gameObject);

            ValidateReferences();
        }

        private void Start()
        {
            if (targetDataSource.Value != null)
            {
                _target = targetDataSource.Value;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & targetLayer.value) != 0)
            {
                _target.ReceiveAttack();

                if (enableLogs) Debug.Log($"{name}: <color=orange> Player has died! </color>");
            }
        }

        private void ValidateReferences()
        {
            if (targetLayer == 0)
            {
                Debug.LogError($"{name}: {nameof(targetLayer)} is not set!");
                return;
            }

            if (!targetDataSource)
            {
                Debug.LogError($"{name}: {nameof(targetDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}