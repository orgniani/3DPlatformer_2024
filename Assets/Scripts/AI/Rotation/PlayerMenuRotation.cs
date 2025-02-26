using System.Collections;
using UnityEngine;
using Scenery;

namespace AI.Rotation
{
    public class PlayerMenuRotation : SimpleRotation
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private SceneryManagerSource sceneryManagerSource;

        [Header("Animator Parameters")]
        [SerializeField] private string waveTriggerParameter = "on_wave";

        private SceneryManager _sceneryManager;
        private Coroutine _waitToStartRotatingCoroutine;

        protected override void Awake()
        {
            base.Awake();

            if (!animator)
            {
                Debug.LogError($"{name}: {nameof(animator)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }

            if (!sceneryManagerSource)
            {
                Debug.LogError($"{name}: {nameof(sceneryManagerSource)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }
        }

        protected override void OnEnable()
        {
            if (sceneryManagerSource.Value)
                _sceneryManager = sceneryManagerSource.Value;

            _waitToStartRotatingCoroutine = StartCoroutine(WaitToStartRotating());
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_waitToStartRotatingCoroutine != null)
                StopCoroutine(_waitToStartRotatingCoroutine);
        }

        private IEnumerator WaitToStartRotating()
        {
            yield return new WaitUntil(() => !_sceneryManager.IsLoading);

            base.OnEnable();
            HandleWave();
        }

        protected override void CompleteRotationAction()
        {
            HandleWave();
        }

        private void HandleWave()
        {
            animator.SetTrigger(waveTriggerParameter);
        }
    }
}
