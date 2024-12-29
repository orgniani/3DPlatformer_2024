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
        [SerializeField] private Image loadingBarFill;

        [Header("Parameters")]
        [SerializeField] private float fillDuration = 0.25f;

        private Coroutine _currentFillCoroutine;
        private SceneryManager _sceneryManager;

        private void Awake()
        {
            _sceneryManager = GetComponent<SceneryManager>();

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
            loadingBarFill.fillAmount = 0;
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

            _currentFillCoroutine = StartCoroutine(LerpFill(loadingBarFill.fillAmount, percentage));
        }

        private IEnumerator LerpFill(float from, float to)
        {
            float startTime = Time.time;
            float endTime = startTime + fillDuration;
            float startFillAmount = loadingBarFill.fillAmount;

            while (Time.time < endTime)
            {
                float timeProgress = (Time.time - startTime) / fillDuration;
                loadingBarFill.fillAmount = Mathf.Lerp(startFillAmount, to, timeProgress);
                yield return null;
            }

            loadingBarFill.fillAmount = to;
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

            if (!loadingBarFill)
            {
                Debug.LogError($"{name}: {nameof(loadingBarFill)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}