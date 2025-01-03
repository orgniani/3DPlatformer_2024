using UnityEngine;
using Gameplay;
using DataSources;

namespace AI
{
    public class Obstacle : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<PlayerManager> playerDataSource;

        [Header("Layers")]
        [SerializeField] private LayerMask playerLayer;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private PlayerManager _target;

        private void Awake()
        {
            ValidateReferences();
        }

        private void Start()
        {
            if (playerDataSource.Value != null)
            {
                _target = playerDataSource.Value;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
            {
                _target.ReceiveAttack();

                if (enableLogs) Debug.Log($"{name}: <color=orange> Player has died! </color>");
            }
        }

        private void ValidateReferences()
        {
            if (playerLayer == 0)
            {
                Debug.LogError($"{name}: {nameof(playerLayer)} is not set!");
                return;
            }

            if (!playerDataSource)
            {
                Debug.LogError($"{name}: {nameof(playerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}