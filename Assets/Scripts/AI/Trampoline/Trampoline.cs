using System.Collections;
using UnityEngine;
using Player.Jump; //TODO: Figure out if theres a better way than Player.Jump
using Events;

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
                //TODO: There must be a better way than TryGetComponent --> SEARCH!
                if (other.TryGetComponent(out PlayerJump playerJump))
                    StartCoroutine(TrampolineBounce(playerJump));
            }
        }

        private IEnumerator TrampolineBounce(PlayerJump playerJump)
        {
            isBouncing = true;

            Vector3 downPosition = initialPosition - new Vector3(0, Model.DepressionAmount, 0);
            yield return MoveTrampoline(Model.BounceSpeed, downPosition);

            Vector3 extraForce = Vector3.up * Model.LaunchForce;
            yield return playerJump.TrampolineJump(extraForce);

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
