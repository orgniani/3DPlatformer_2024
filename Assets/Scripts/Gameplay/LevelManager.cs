using System.Collections;
using UnityEngine;
using DataSources;
using Characters;

namespace Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<Character> playerDataSource;

        [Header("Transforms")]
        [SerializeField] private Transform levelStart;

        private Character _player;

        private void Awake()
        {
            ValidateReferences();
        }

        private IEnumerator Start()
        {
            //TODO: This could be used in other implementations of data sources
            while (_player == null)
            {
                if (playerDataSource.Value != null)
                    _player = playerDataSource.Value;

                yield return null;
            }

            _player.SetStartPosition(levelStart.position, levelStart.rotation);
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

