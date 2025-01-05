using System.Collections;
using UnityEngine;
using DataSources;
using Core.Interactions;

namespace Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<PlayerManager> playerDataSource;

        [Header("Transforms")]
        [SerializeField] private Transform levelStart;

        private PlayerManager _playerManager;

        private void Awake()
        {
            ValidateReferences();
        }

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private IEnumerator Start()
        {
            while (_playerManager == null)
            {
                if (playerDataSource.Value != null)
                    _playerManager = playerDataSource.Value;

                yield return null;
            }

            _playerManager.SetPlayerAtLevelStartAndEnable(levelStart.position);
        }

        private void ValidateReferences()
        {
            if (!playerDataSource)
            {
                Debug.LogError($"{name}: {nameof(playerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!levelStart)
            {
                Debug.LogError($"{name}: {nameof(levelStart)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}

