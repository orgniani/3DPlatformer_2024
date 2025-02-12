using System.Collections;
using UnityEngine;
using Events;
using Core.Interactions;

namespace AI.Trampoline
{
    public class Trampoline : MonoBehaviour
    {
        private Vector3 initialPosition;
        private bool isBouncing = false;

        public TrampolineModel Model { get; set; }

        private void Start()
        {
            initialPosition = transform.localPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & Model.PlayerLayer.value) != 0 && !isBouncing)
            {
                if (other.TryGetComponent(out IBounceable bounceable))
                    StartCoroutine(TrampolineBounce(bounceable));
            }
        }

        private IEnumerator TrampolineBounce(IBounceable bounceable)
        {
            isBouncing = true;

            Vector3 downPosition = initialPosition - new Vector3(0, Model.DepressionAmount, 0);
            yield return MoveTrampoline(Model.BounceSpeed, downPosition);

            Vector3 bounceForce = Vector3.up * Model.LaunchForce;
            yield return bounceable.TrampolineBounce(bounceForce);

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, Model.BounceAudio, gameObject);

            Vector3 upPosition = initialPosition + new Vector3(0, Model.ElevationAmount, 0);
            yield return MoveTrampoline(Model.BounceSpeed, upPosition);

            yield return MoveTrampoline(Model.RecoverySpeed, initialPosition);

            isBouncing = false;
        }

        private IEnumerator MoveTrampoline(float speed, Vector3 targetPosition)
        {
            float elapsedTime = 0f;
            Vector3 startPosition = transform.localPosition;

            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * speed;
                float curveValue = Model.BounceCurve.Evaluate(elapsedTime);

                transform.localPosition = Vector3.Lerp(startPosition, targetPosition, curveValue);

                yield return new WaitForFixedUpdate();
            }

            transform.localPosition = targetPosition;
        }
    }
}
