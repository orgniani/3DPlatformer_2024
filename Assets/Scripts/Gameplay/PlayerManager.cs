//TODO: using Player;
using DataSources;
using Events;
using UnityEngine;
using Core;
using UnityEngine.TextCore.Text;

namespace Gameplay
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<PlayerManager> playerDataSource;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private void Awake()
        {
            if (!playerDataSource)
            {
                Debug.LogError($"{name}: {nameof(playerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            if (playerDataSource.Value == null)
                playerDataSource.Value = this;

        }

        private void OnDisable()
        {
            if (playerDataSource.Value == this)
                playerDataSource.Value = null;
        }

        public void SetPlayerAtLevelStartAndEnable(Vector3 levelStartPosition)
        {
            transform.position = levelStartPosition;
        }

        public void ReceiveAttack()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.LoseAction, true);

            if (enableLogs) Debug.Log($"<color=red> {name}: received an attack! </color>");
            Destroy(gameObject);
        }
    }
}

