using System.Collections;
using UnityEngine;
using Events;

namespace AI.Movement
{
    public class SimpleMovementController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject obstacle;
        [SerializeField] private Transform position1;
        [SerializeField] private Transform position2;

        private Coroutine _movementCoroutine;

        public SimpleMovementModel Model { get; set; }

        private void Awake()
        {
            ValidateReferences();
        }

        private void Start()
        {
            _movementCoroutine = StartCoroutine(MoveSideToSide());
        }

        private void OnDisable()
        {
            if (_movementCoroutine != null)
                StopCoroutine(_movementCoroutine);
        }

        private IEnumerator MoveSideToSide()
        {
            float elapsedTime = 0f;
            float lastFrameCurveValue = 0f;

            while (enabled)
            {
                elapsedTime += Time.deltaTime * Model.Speed;
                float curveValue = Model.MovementCurve.Evaluate(elapsedTime);

                if (lastFrameCurveValue < Model.ImpactThreshold && curveValue >= Model.ImpactThreshold)
                {
                    if (EventManager<string>.Instance)
                        EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, Model.ImpactAudio, gameObject);
                }

                obstacle.transform.position = Vector3.Lerp(position1.position, position2.position, curveValue);
                lastFrameCurveValue = curveValue;

                yield return null;
            }
        }

        private void ValidateReferences()
        {
            if (!obstacle)
            {
                Debug.LogError($"{name}: {nameof(obstacle)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!position1)
            {
                Debug.LogError($"{name}: {nameof(position1)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!position2)
            {
                Debug.LogError($"{name}: {nameof(position2)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}
