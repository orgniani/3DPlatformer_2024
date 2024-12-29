using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataSources;
using Core;
using Events;
using System.Linq;

namespace Scenery
{
    public class SceneryManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<SceneryManager> sceneryManagerDataSource;

        [Header("Parameters")]
        [Header("Loading time")]
        [SerializeField] private float fakeLoadingTime = 1;
        [SerializeField] private float delayPerScene = 0.5f;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private SceneryLoadId[] _allScenesIds;
        private int[] _currentLevelIds;

        public event Action OnLoadStart = delegate { };
        public event Action<float> OnLoadPercentage = delegate { };
        public event Action OnLoadEnd = delegate { };

        private void Awake()
        {
            if (!sceneryManagerDataSource)
            {
                Debug.LogError($"{name}: {nameof(sceneryManagerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.LoadScenery, HandleLoadScenery);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.UnloadScenery, HandleUnloadScenery);
            }

            sceneryManagerDataSource.Value = this;
        }

        private void OnDisable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.LoadScenery, HandleLoadScenery);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.UnloadScenery, HandleUnloadScenery);
            }

            if (sceneryManagerDataSource.Value == this)
                sceneryManagerDataSource.Value = null;
        }

        public void SetUp(SceneryLoadId[] sceneryLoadIds)
        {
            _currentLevelIds = new int[0];
            _allScenesIds = sceneryLoadIds;
        }

        public void ResetIdsToIndex0()
        {
            _currentLevelIds = _allScenesIds[0].SceneIndexes;
        }

        private void HandleLoadScenery(params object[] args)
        {
            if (args.Length > 0 && args[0] is int[] newSceneIndexes)
            {
                if (_currentLevelIds != null)
                {
                    StartCoroutine(UnloadAndLoadScenes(_currentLevelIds, newSceneIndexes));
                }

                _currentLevelIds = newSceneIndexes;
            }
        }

        private void HandleUnloadScenery(params object[] args)
        {
            if (args.Length > 0 && args[0] is int[] newSceneIndexes)
            {
                if (_currentLevelIds != null)
                {
                    _currentLevelIds = new int[0];
                    StartCoroutine(UnloadAndLoadScenes(newSceneIndexes, _currentLevelIds));
                }
            }
        }

        private IEnumerator UnloadAndLoadScenes(int[] unloadSceneIndexes, int[] loadSceneIndexes)
        {
            OnLoadStart?.Invoke();
            OnLoadPercentage?.Invoke(0);

            int unloadCount = unloadSceneIndexes.Length;
            int loadCount = loadSceneIndexes.Length;
            int totalCount = unloadCount + loadCount;

            if (unloadSceneIndexes.Length > 0)
            {
                yield return Unload(unloadSceneIndexes, currentIndex => OnLoadPercentage((float)currentIndex / totalCount));
                yield return new WaitForSeconds(fakeLoadingTime);
            }

            if (loadSceneIndexes.Length > 0)
            {
                yield return Load(loadSceneIndexes, currentIndex => OnLoadPercentage((float)(currentIndex + unloadCount) / totalCount));
                yield return new WaitForSeconds(fakeLoadingTime);
            }

            _currentLevelIds = loadSceneIndexes;
            OnLoadEnd?.Invoke();
        }

        private IEnumerator Load(int[] sceneIndexes, Action<float> onLoadedSceneQtyChanged)
        {
            var current = 0;

            foreach (var sceneIndex in sceneIndexes)
            {
                var loadOp = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

                if (loadOp == null)
                {
                    if (enableLogs) Debug.LogError($"Failed to load scene at index {sceneIndex}");
                    continue;
                }

                while (!loadOp.isDone)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(delayPerScene);

                current++;
                onLoadedSceneQtyChanged?.Invoke((float)current / sceneIndexes.Length);
            }
        }

        private IEnumerator Unload(int[] sceneIndexes, Action<float> onLoadedSceneQtyChanged)
        {
            var current = 0;

            foreach (var sceneIndex in sceneIndexes)
            {
                var sceneryLoadId = _allScenesIds.FirstOrDefault(s => s.SceneIndexes.Contains(sceneIndex));
                if (sceneryLoadId != null && !sceneryLoadId.CanUnload) continue;

                if (SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded)
                {
                    var unloadOp = SceneManager.UnloadSceneAsync(sceneIndex);

                    if (unloadOp == null)
                    {
                        if (enableLogs) Debug.LogError($"Failed to unload scene at index {sceneIndex}");
                        continue;
                    }

                    while (!unloadOp.isDone)
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(delayPerScene);

                    current++;
                    onLoadedSceneQtyChanged?.Invoke(current);
                }

                else
                {
                    if (enableLogs) Debug.Log($"<color=purple> Scene at index {sceneIndex} is not currently loaded.\n" +
                                              $"Skipping unload operation. </color>");
                }
            }
        }
    }
}