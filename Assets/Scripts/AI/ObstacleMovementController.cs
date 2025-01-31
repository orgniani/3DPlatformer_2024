using System.Collections;
using UnityEngine;
using Audio;
using Events;

namespace AI
{
    //TODO: Change name --> be more specific
    public class ObstacleMovementController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject obstacle;
        [SerializeField] private Transform position1;
        [SerializeField] private Transform position2;

        [Header("Audio")]
        [SerializeField] private AudioEvent impactAudio;

        [Header("Parameters")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private AnimationCurve movementCurve;

        private Coroutine _movementCoroutine;

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
            float lastPingPongValue = 0f;

            while (true) //TODO: NOT WHILE(TRUE), find alternative
            {
                //TODO: Pingpong value???
                float pingPongValue = Mathf.PingPong(Time.time * moveSpeed, 1f);
                float curveValue = movementCurve.Evaluate(pingPongValue);

                //TODO: Get rid of hardcoded values
                if (lastPingPongValue < 0.95f && pingPongValue >= 0.95f)
                {
                    if (EventManager<string>.Instance)
                        EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, impactAudio, gameObject);
                }

                obstacle.transform.position = Vector3.Lerp(position1.position, position2.position, curveValue);

                lastPingPongValue = pingPongValue;
                yield return new WaitForFixedUpdate();
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
