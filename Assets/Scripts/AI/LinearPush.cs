using System.Collections;
using UnityEngine;

namespace AI
{
    [RequireComponent((typeof(Rigidbody)))]
    public class LinearPush : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private Vector3 targetOffset = new Vector3(0, 0, 0.2f);
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float movementDuration = 1f;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;
        
        private Rigidbody _rigidBody;
        private Vector3 _targetPosition;
        private Vector3 _movementDirection;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _targetPosition = transform.position + targetOffset;

            _movementDirection = (_targetPosition - transform.position).normalized;
        }

        public IEnumerator PushForward()
        {
            float elapsedTime = 0f;
            _rigidBody.isKinematic = false;

            while (elapsedTime < movementDuration)
            {
                Vector3 force = _movementDirection * moveSpeed;

                _rigidBody.AddForce(force, ForceMode.Force);

                elapsedTime += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }

            OnMovementComplete();
        }

        private void OnMovementComplete()
        {
            if (enableLogs) Debug.Log($"{name}: Push complete!");
            enabled = false;
        }
    }
}
