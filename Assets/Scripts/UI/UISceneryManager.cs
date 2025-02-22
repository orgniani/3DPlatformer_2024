using System.Collections;
using UnityEngine;
using Scenery;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(SceneryManager))]
    public class UISceneryManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Canvas loadingScreen;
        [SerializeField] private Slider loadBar;

        private float _fillDuration;

        private Coroutine _currentFillCoroutine;
        private SceneryManager _sceneryManager;

        private void Awake()
        {
            _sceneryManager = GetComponent<SceneryManager>();
            _fillDuration = _sceneryManager.FakeLoadingTime;

            ValidateReferences();
        }

        private void OnEnable()
        {
            _sceneryManager.OnLoadStart += EnableLoadingScreen;
            _sceneryManager.OnLoadEnd += DisableLoadingScreen;
            _sceneryManager.OnLoadPercentage += UpdateLoadBarFill;
        }

        private void OnDisable()
        {
            _sceneryManager.OnLoadStart -= EnableLoadingScreen;
            _sceneryManager.OnLoadEnd -= DisableLoadingScreen;
            _sceneryManager.OnLoadPercentage -= UpdateLoadBarFill;
        }

        private void EnableLoadingScreen()
        {
            loadingScreen.enabled = true;
            loadBar.value = 0;
        }

        private void DisableLoadingScreen()
        {
            if (_currentFillCoroutine != null)
                StopCoroutine(_currentFillCoroutine);

            loadingScreen.enabled = false;
        }

        private void UpdateLoadBarFill(float percentage)
        {
            if (_currentFillCoroutine != null)
                StopCoroutine(_currentFillCoroutine);

            _currentFillCoroutine = StartCoroutine(LerpFill(loadBar.value, percentage));
        }

        private IEnumerator LerpFill(float from, float to)
        {
            float startTime = Time.time;
            float endTime = startTime + _fillDuration;
            float startFillAmount = loadBar.value;

            while (Time.time < endTime)
            {
                float timeProgress = (Time.time - startTime) / _fillDuration;
                loadBar.value = Mathf.Lerp(startFillAmount, to, timeProgress);
                yield return null;
            }

            loadBar.value = to;
        }

        private void ValidateReferences()
        {
            if (!loadingScreen)
            {
                Debug.LogError($"{name}: {nameof(loadingScreen)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!loadBar)
            {
                Debug.LogError($"{name}: {nameof(loadBar)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}