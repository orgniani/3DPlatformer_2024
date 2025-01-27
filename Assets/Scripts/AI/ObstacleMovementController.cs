using System.Collections;
using UnityEngine;

namespace AI
{
    //TODO: Change name --> be more specific
    public class ObstacleMovementController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject obstacle;
        [SerializeField] private Transform position1;
        [SerializeField] private Transform position2;

        [Header("Parameters")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private AnimationCurve movementCurve;

        private Coroutine movementCoroutine;

        private void Awake()
        {
            ValidateReferences();
        }

        private void Start()
        {
            movementCoroutine = StartCoroutine(MoveSideToSide());
        }

        private void OnDisable()
        {
            if (movementCoroutine != null)
                StopCoroutine(movementCoroutine);
        }

        private IEnumerator MoveSideToSide()
        {
            while (true) //TODO: NOT WHILE(TRUE), find alternative
            {
                float pingPongValue = Mathf.PingPong(Time.time * moveSpeed, 1f);
                float curveValue = movementCurve.Evaluate(pingPongValue);

                obstacle.transform.position = Vector3.Lerp(position1.position, position2.position, curveValue);

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
