using System.Collections;
using UnityEngine;

namespace AI
{
    public class Trampoline : MonoBehaviour
    {
        [Header("Trampoline Settings")]
        [SerializeField] private float depressionAmount = 0.2f;
        [SerializeField] private float upAmount = 1.5f;

        [SerializeField] private float depressionSpeed = 5f;
        [SerializeField] private float backToInitialPositionSpeed = 2f;
        [SerializeField] private float launchForceUp = 10f;

        [SerializeField] private AnimationCurve bounceCurve;

        [SerializeField] private LayerMask targetLayer;

        private Vector3 initialPosition;
        private bool isBouncing = false;

        private void Start()
        {
            initialPosition = transform.localPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & targetLayer.value) != 0 && !isBouncing)
            {
                if (other.TryGetComponent<Rigidbody>(out Rigidbody playerRigidbody))
                    StartCoroutine(TrampolineBounce(playerRigidbody));
            }
        }

        private IEnumerator TrampolineBounce(Rigidbody playerRigidbody)
        {
            isBouncing = true;

            // Step 1: Move the trampoline down
            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * depressionSpeed;
                float curveValue = bounceCurve.Evaluate(elapsedTime);
                transform.localPosition = initialPosition - new Vector3(0, curveValue * depressionAmount, 0);
                yield return null;
            }

            // Step 2: Apply force to the player and move trampoline way up
            elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * depressionSpeed;
                float curveValue = bounceCurve.Evaluate(elapsedTime);
                transform.localPosition = initialPosition + new Vector3(0, curveValue * upAmount, 0); // Move way up
                yield return null;
            }

            Vector3 launchDirection = transform.up * launchForceUp;
            playerRigidbody.AddForce(launchDirection, ForceMode.Impulse);

            // Step 3: Smoothly return the trampoline to its original position
            elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * (backToInitialPositionSpeed);
                float curveValue = bounceCurve.Evaluate(elapsedTime);
                transform.localPosition = Vector3.Lerp(
                    transform.localPosition,
                    initialPosition,
                    curveValue
                );
                yield return null;
            }

            transform.localPosition = initialPosition;
            isBouncing = false;
        }
    }
}
