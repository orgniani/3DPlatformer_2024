using Events;
using UnityEngine;
using Player.Brain;
using Camera; // TODO: There must be a better way
using System.Collections;

namespace Input.Cheats
{
    public class GodModeFlightController : MonoBehaviour
    {
        //TODO: REVISIT WHOLE SCRIPT!!! --> GOD MODE
        [SerializeField] private PlayerBrain playerBrain;
        [SerializeField] private Rigidbody playerRigidbody;

        [SerializeField] private float flightSpeed = 10f;
        [SerializeField] private float verticalSpeed = 5f;

        private Vector2 _movementInput;
        private float _flightInput;

        private Coroutine _flightCoroutine;

        private void Awake()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            ValidateReferences();

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.FlightAction, HandleFlightInput);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.MoveAction, HandleMoveInput);
            }

            if (_flightCoroutine == null)
                _flightCoroutine = StartCoroutine(FlightLoop());
        }

        private void OnDisable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.FlightAction, HandleFlightInput);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.MoveAction, HandleMoveInput);
            }
        }

        private void HandleFlightInput(params object[] args)
        {
            if (args.Length > 0 && args[0] is Vector2 flightInput)
                _flightInput = flightInput.y;
        }

        private void HandleMoveInput(params object[] args)
        {
            if (args.Length > 0 && args[0] is Vector2 movementInput)
                _movementInput = movementInput;
        }

        private IEnumerator FlightLoop()
        {
            while (enabled)
            {
                Vector3 horizontalMovement = TransformDirectionRelativeToCamera(_movementInput);

                Vector3 velocity = playerRigidbody.velocity;
                velocity.x = horizontalMovement.x * flightSpeed;
                velocity.z = horizontalMovement.z * flightSpeed;
                velocity.y = _flightInput * verticalSpeed;

                if(!playerRigidbody.isKinematic) playerRigidbody.velocity = velocity;

                yield return new WaitForFixedUpdate();
            }
        }

        private Vector3 TransformDirectionRelativeToCamera(Vector2 input)
        {
            Vector3 direction = new Vector3(input.x, 0, input.y);

            Vector3 cameraForward = playerBrain.Camera.transform.forward;
            cameraForward.y = 0;

            direction = Quaternion.LookRotation(cameraForward) * direction;

            return direction.normalized;
        }

        public void StopFlight()
        {
            if (!playerRigidbody) return;
            if (_flightCoroutine == null) return;

            playerRigidbody.useGravity = true;

            StopCoroutine(_flightCoroutine);
            _flightCoroutine = null;

            enabled = false;
        }

        private void ValidateReferences()
        {
            if (!playerBrain)
            {
                Debug.LogError($"{name}: {nameof(playerBrain)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
            if (!playerRigidbody)
            {
                Debug.LogError($"{name}: {nameof(playerRigidbody)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}