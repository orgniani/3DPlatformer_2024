using System.Collections;
using UnityEngine;
using Player.Jump;
using Audio;
using Events;

namespace AI
{
    public class Trampoline : MonoBehaviour
    {
        [Header("Trampoline Settings")]
        [SerializeField] private float depressionAmount = 0.2f;
        [SerializeField] private float upAmount = 1.5f;

        [SerializeField] private float bounceSpeed = 5f;
        [SerializeField] private float backToInitialPositionSpeed = 2f;
        [SerializeField] private float launchForceUp = 10f;

        [SerializeField] private AnimationCurve bounceCurve;

        [SerializeField] private LayerMask targetLayer;

        [SerializeField] private AudioEvent trampolineAudio;

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
                //TODO: There must be a better way than TryGetComponent --> SEARCH!
                if (other.TryGetComponent<PlayerJump>(out PlayerJump playerJump))
                {
                    StartCoroutine(TrampolineBounce(playerJump));
                }
            }
        }

        private IEnumerator TrampolineBounce(PlayerJump playerJump)
        {
            isBouncing = true;

            Vector3 downPosition = initialPosition - new Vector3(0, depressionAmount, 0);
            yield return SmoothTrampolineMovement(bounceSpeed, depressionAmount, downPosition);

            Vector3 extraForce = Vector3.up * launchForceUp;
            yield return playerJump.TriggerJump(extraForce);

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, trampolineAudio, gameObject);

            Vector3 upPosition = initialPosition + new Vector3(0, upAmount, 0);
            yield return SmoothTrampolineMovement(bounceSpeed, upAmount, upPosition);

            yield return SmoothTrampolineMovement(backToInitialPositionSpeed, depressionAmount, initialPosition);

            isBouncing = false;
        }

        //TODO: Check for concistency in ALL animation curve scripts
        private IEnumerator SmoothTrampolineMovement(float movementSpeed, float movementAmount, Vector3 targetPosition)
        {
            float elapsedTime = 0f;
            Vector3 startPosition = transform.localPosition;

            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * movementSpeed;
                float curveValue = bounceCurve.Evaluate(elapsedTime);

                transform.localPosition = Vector3.Lerp(startPosition, targetPosition, curveValue);

                yield return null;
            }

            transform.localPosition = targetPosition;
        }

    }
}
