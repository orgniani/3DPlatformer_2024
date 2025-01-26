using System.Collections;
using UnityEngine;

namespace AI
{
    [RequireComponent((typeof(Rigidbody)))]
    public class ObstacleMovement : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private Vector3 targetOffset = new Vector3(0, 0, 0.2f);
        [SerializeField] private float moveSpeed = 2f;

        [SerializeField] private float movementDuration = 1f;

        private Rigidbody _rigidBody;
        private Vector3 targetPosition;
        private Vector3 movementDirection;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            targetPosition = transform.position + targetOffset;

            movementDirection = (targetPosition - transform.position).normalized; // Direction towards target
        }

        public IEnumerator PushForward()
        {
            float elapsedTime = 0f;
            _rigidBody.isKinematic = false;

            while (elapsedTime < movementDuration)
            {
                Vector3 force = movementDirection * moveSpeed;

                _rigidBody.AddForce(force, ForceMode.Force);

                elapsedTime += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }

            OnMovementComplete();
        }

        private void OnMovementComplete()
        {
            if (enableLogs) Debug.Log($"{name}: Movement complete.");
            enabled = false;
        }
    }
}
