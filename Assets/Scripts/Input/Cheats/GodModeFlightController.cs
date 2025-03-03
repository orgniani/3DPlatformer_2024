using Events;
using UnityEngine;
using Camera;
using System.Collections;
using DataSources;
using Characters;

namespace Input.Cheats
{
    public class GodModeFlightController : MonoBehaviour
    {
        [Header("Data Sources")]
        [SerializeField] private DataSource<CameraSetup> cameraDataSource;
        [SerializeField] private DataSource<Character> playerDataSource;

        [Header("Parameters")]
        [SerializeField] private float flightSpeed = 10f;
        [SerializeField] private float verticalSpeed = 5f;

        private Vector2 _movementInput;
        private float _flightInput;

        private CameraSetup _playerCamera;
        private Rigidbody _playerRigidbody;

        private Coroutine _flightCoroutine;

        private void Awake()
        {
            if (cameraDataSource.Value != null)
                _playerCamera = cameraDataSource.Value;

            enabled = false;
        }

        private void OnEnable()
        {
            ValidateReferences();

            if (!playerDataSource.Value)
            {
                enabled = false;
                return;
            }

            _playerRigidbody = playerDataSource.Value.CharacterRigidBody;

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
                if (!_playerRigidbody)
                {
                    enabled = false;
                    _flightCoroutine = null;
                    yield break;
                }

                Vector3 horizontalMovement = TransformDirectionRelativeToCamera(_movementInput);

                Vector3 velocity = _playerRigidbody.velocity;
                velocity.x = horizontalMovement.x * flightSpeed;
                velocity.z = horizontalMovement.z * flightSpeed;
                velocity.y = _flightInput * verticalSpeed;

                if(!_playerRigidbody.isKinematic) _playerRigidbody.velocity = velocity;

                yield return new WaitForFixedUpdate();
            }
        }

        private Vector3 TransformDirectionRelativeToCamera(Vector2 input)
        {
            Vector3 direction = new Vector3(input.x, 0, input.y);

            Vector3 cameraForward = _playerCamera.transform.forward;
            cameraForward.y = 0;

            direction = Quaternion.LookRotation(cameraForward) * direction;

            return direction.normalized;
        }

        public void StopFlight()
        {
            if (!_playerRigidbody) return;
            if (_flightCoroutine == null) return;

            _playerRigidbody.useGravity = true;

            StopCoroutine(_flightCoroutine);
            _flightCoroutine = null;

            enabled = false;
        }

        private void ValidateReferences()
        {
            if (!playerDataSource)
            {
                Debug.LogError($"{name}: {nameof(playerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!cameraDataSource)
            {
                Debug.LogError($"{name}: {nameof(cameraDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}