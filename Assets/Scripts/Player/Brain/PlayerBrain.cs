using Player.Body;
using Player.Jump;
using Events;
using UnityEngine;
using Camera;

namespace Player.Brain
{
    public class PlayerBrain : MonoBehaviour
    {
        [Header("References")]
        [Header("Player")]
        [SerializeField] private PlayerBody body;
        [SerializeField] private PlayerJump jump;

        private Vector3 _desiredDirection;
        private Vector2 _input;

        private float _acceleration;

        public CameraSetup Camera { get; set; }

        public BrainModel Model { get; set; }

        public float Acceleration { set { _acceleration = value; } }

        private void Awake()
        {
            ValidateReferences();
        }

        private void OnEnable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.MoveAction, HandleMovementInput);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.JumpAction, HandleJumpInput);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.LookAction, HandleCameraInput);
            }
        }

        private void OnDisable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.MoveAction, HandleMovementInput);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.JumpAction, HandleJumpInput);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.LookAction, HandleCameraInput);
            }
        }

        private void Update()
        {
            if (_desiredDirection.magnitude > Mathf.Epsilon && _input.magnitude < Mathf.Epsilon)
                body.RequestBrake(Model.MovementBreakMultiplier);

            Vector3 movementInput = _input;
            _desiredDirection = TransformDirectionRelativeToCamera(movementInput);
            body.SetMovement(new MovementRequest(_desiredDirection, Model.Speed, _acceleration));
        }

        private Vector3 TransformDirectionRelativeToCamera(Vector2 input)
        {
            Vector3 direction = new Vector3(input.x, 0, input.y);

            Vector3 cameraForward = Camera.transform.forward;
            cameraForward.y = 0;

            direction = Quaternion.LookRotation(cameraForward) * direction;

            return direction.normalized;
        }

        private void HandleMovementInput(params object[] args)
        {
            if (args.Length > 0 && args[0] is Vector2 movementInput)
                _input = movementInput;
        }

        private void HandleCameraInput(params object[] args)
        {
            if (args.Length > 0 && args[0] is Vector2 cameraInput)
                Camera.SetInputRotation(cameraInput);
        }

        private void HandleJumpInput(params object[] args)
        {
            jump.TryJump(Model.Acceleration);
        }

        private void ValidateReferences()
        {
            if (!body)
            {
                Debug.LogError($"{name}: {nameof(body)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!jump)
            {
                Debug.LogError($"{name}: {nameof(jump)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}