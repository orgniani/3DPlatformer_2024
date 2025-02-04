using Events;
using UnityEngine;
using Player.Brain;
using Camera; // TODO: There must be a better way

namespace Input
{
    public class GodModeFlightController : MonoBehaviour
    {
        //TODO: REVISIT WHOLE SCRIPT!!! --> GOD MODE
        [SerializeField] private PlayerBrain playerBrain;
        [SerializeField] private Rigidbody playerRigidbody;  // Assign your player here
        [SerializeField] private float flightSpeed = 10f;
        [SerializeField] private float verticalSpeed = 5f;

        private Vector2 _moveInput;  // Stores horizontal movement input (WASD)
        private float _verticalInput;  // Stores vertical movement input (Q/E)

        private void Awake()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.FlightAction, HandleFlightInput);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.MoveAction, HandleMoveInput);
            }
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
                _verticalInput = flightInput.y;  // Store Q/E input (vertical movement)
        }

        // Handle horizontal movement (WASD for forward, backward, side-to-side)
        private void HandleMoveInput(params object[] args)
        {
            if (args.Length > 0 && args[0] is Vector2 moveInput)
                _moveInput = moveInput;  // Store WASD input (horizontal movement)
        }

        private void FixedUpdate()
        {
            if (playerRigidbody == null || !enabled) return;  // Only process if enabled

            // Apply horizontal movement (WASD) relative to the camera's direction
            Vector3 horizontalMovement = TransformDirectionRelativeToCamera(_moveInput);

            // Apply vertical movement (Q/E)
            Vector3 velocity = playerRigidbody.velocity;
            velocity.x = horizontalMovement.x * flightSpeed;  // Side-to-side movement
            velocity.z = horizontalMovement.z * flightSpeed;  // Forward/backward movement
            velocity.y = _verticalInput * verticalSpeed;  // Ascend/Descend (Q/E)

            playerRigidbody.velocity = velocity;
        }

        // Transform horizontal input into camera-relative movement direction
        private Vector3 TransformDirectionRelativeToCamera(Vector2 input)
        {
            // Create a direction vector (X = left/right, Z = forward/back)
            Vector3 direction = new Vector3(input.x, 0, input.y);  // Ignore Y for horizontal movement

            // Get the camera's forward direction and zero out its Y component
            Vector3 cameraForward = playerBrain.Camera.transform.forward;
            cameraForward.y = 0;  // Ignore the vertical (Y) component

            // Apply camera rotation to the direction vector
            direction = Quaternion.LookRotation(cameraForward) * direction;

            return direction.normalized;
        }

        public void StopFlight()
        {
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector3.zero;
                playerRigidbody.useGravity = true;
                enabled = false;
            }
        }
    }
}