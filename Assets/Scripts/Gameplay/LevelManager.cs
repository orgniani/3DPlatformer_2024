using System.Collections;
using UnityEngine;
using DataSources;
using Characters;
using Scenery;

namespace Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<LevelManager> levelManagerDataSource;
        [SerializeField] private DataSource<SceneryManager> sceneryManagerDataSource;
        [SerializeField] private DataSource<Character> playerDataSource;

        [Header("Transforms")]
        [SerializeField] private Transform levelStart;

        private Character _player;
        private SceneryManager _sceneryManager;

        private void Awake()
        {
            ValidateReferences();
        }

        private void OnEnable()
        {
            if (sceneryManagerDataSource.Value != null)
                _sceneryManager = sceneryManagerDataSource.Value;
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

            yield return new WaitUntil(() => !_sceneryManager.IsLoading);

            levelManagerDataSource.Value = this;
        }

        private void OnDisable()
        {
            if (levelManagerDataSource.Value == this)
                levelManagerDataSource.Value = null;
        }

        private void ValidateReferences()
        {
            if (!levelManagerDataSource)
            {
                Debug.LogError($"{name}: {nameof(levelManagerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!sceneryManagerDataSource)
            {
                Debug.LogError($"{name}: {nameof(sceneryManagerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

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

