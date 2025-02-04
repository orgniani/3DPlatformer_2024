using UnityEngine;
using DataSources;
using Scenery;
using Events;
using System.Collections.Generic;
using System.Collections;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<GameManager> gameManagerDataSource;
        [SerializeField] private DataSource<SceneryManager> sceneryManagerDataSource;

        [Header("Scenery Ids")]
        [Tooltip("Contains scene indexes for the Menus and Managers.")]
        [SerializeField] private SceneryLoadId firstBatch;

        [Tooltip("Contains the scene index for the World scene.")]
        [SerializeField] private SceneryLoadId secondBatch;

        [Tooltip("Contains scene indexes for each level. Each Scenery Model corresponds to a specific level.")]
        [SerializeField] private SceneryLoadId[] levels;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private SceneryManager _sceneryManager;
        private SceneryLoadId[] _allSceneIds;

        private int _currentLevelIndex = 0;

        public bool IsFinalLevel { get; private set; }
        public bool IsGamePaused { get; private set; }

        private void Awake()
        {
            ValidateReferences();

            _allSceneIds = new SceneryLoadId[2 + levels.Length];

            _allSceneIds[0] = firstBatch;
            _allSceneIds[1] = secondBatch;

            for (int i = 0; i < levels.Length; i++)
            {
                _allSceneIds[2 + i] = levels[i];
            }

            var sceneIndexes = new List<int>();

            foreach (var sceneId in _allSceneIds)
            {
                foreach (var index in sceneId.SceneIndexes)
                {
                    if (sceneIndexes.Contains(index))
                    {
                        if (enableLogs) Debug.LogWarning($"{name}: Scene index {index} has already been added!");
                        continue;
                    }
                    sceneIndexes.Add(index);
                }
            }
        }

        private void OnEnable()
        {
            gameManagerDataSource.Value = this;

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.WinAction, OnWinLevel);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.LoseAction, OnGameOver);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.PauseAction, HandlePauseGame);
            }
        }

        private void Start()
        {
            IsFinalLevel = false;

            if (sceneryManagerDataSource.Value != null)
            {
                _sceneryManager = sceneryManagerDataSource.Value;
                _sceneryManager.SetUp(_allSceneIds);
            }


            UnloadAllScenesOnStart();

            InvokeLoadSceneryEvent(firstBatch.SceneIndexes);
        }

        private void OnDisable()
        {
            if (gameManagerDataSource != null && gameManagerDataSource.Value == this)
                gameManagerDataSource.Value = null;

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.WinAction, OnWinLevel);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.LoseAction, OnGameOver);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.PauseAction, HandlePauseGame);
            }
        }

        private void UnloadAllScenesOnStart()
        {
            foreach (var id in _allSceneIds)
            {
                if (!id.CanUnload)
                {
                    id.CanUnload = true;
                    InvokeUnloadSceneryEvent(id.SceneIndexes);

                    id.CanUnload = false;

                    continue;
                }

                InvokeUnloadSceneryEvent(id.SceneIndexes);
            }
        }

        private void OnWinLevel(params object[] args)
        {
            if (!IsFinalLevel) NextLevel();
        }

        public void OnGameOver(params object[] args)
        {
            InvokeUnloadSceneryEvent(levels[_currentLevelIndex].SceneIndexes);
            InvokeUnloadSceneryEvent(secondBatch.SceneIndexes);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void HandlePauseGame(params object[] args)
        {
            if (_sceneryManager.IsLoading) return; //TODO: Is there a better way to code this?

            IsGamePaused = !IsGamePaused;
            if (IsGamePaused)
            {
                Time.timeScale = 0f;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
            else
            {
                Time.timeScale = 1f;

                //TODO: Check if this works on build :)
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void HandlePlayGame()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            IsFinalLevel = false;

            _sceneryManager.ResetIdsToIndex0();

            _currentLevelIndex = 0; //TODO: Check so that this doesnt replay the TUTORIAL level
            //TODO: Maybe with a TUTORIALDONE flag in here that the endOfTutorial can turn to true using the data source? Mayhaps.
            InvokeLoadSceneryEvent(secondBatch.SceneIndexes);
            InvokeLoadSceneryEvent(levels[_currentLevelIndex].SceneIndexes);
        }

        private void NextLevel()
        {
            if (_currentLevelIndex < levels.Length - 1)
            {
                _currentLevelIndex++;
                InvokeLoadSceneryEvent(levels[_currentLevelIndex].SceneIndexes);
            }

            else
            {
                IsFinalLevel = true;
                OnGameOver();
            }
        }

        private void InvokeLoadSceneryEvent(int[] sceneIndexes)
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.LoadScenery, sceneIndexes);
        }

        private void InvokeUnloadSceneryEvent(int[] sceneIndexes)
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.UnloadScenery, sceneIndexes);
        }

        private void ValidateReferences()
        {
            if (!gameManagerDataSource)
            {
                Debug.LogError($"{name}: {nameof(gameManagerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!firstBatch)
            {
                Debug.LogError($"{name}: {nameof(firstBatch)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!secondBatch)
            {
                Debug.LogError($"{name}: {nameof(secondBatch)} is null!" +
                               $"\nDisabling ocomponentbject to avoid errors.");
                enabled = false;
                return;
            }

            foreach (SceneryLoadId level in levels)
            {
                if (level == null)
                {
                    Debug.LogError($"{name}: a {nameof(level)} is null!" +
                                   $"\nDisabling component to avoid errors.");
                    enabled = false;
                }
            }

            if (levels.Length <= 0)
            {
                Debug.LogError($"{name}: the array of {nameof(levels)} is empty!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}