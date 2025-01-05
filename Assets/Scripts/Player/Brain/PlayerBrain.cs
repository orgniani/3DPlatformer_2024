using Player.Body;
using Player.Jump;
using Camera.FollowTarget;
using Events;
using Core;
using UnityEngine;

namespace Player.Brain
{
    public class PlayerBrain : MonoBehaviour
    {
        [Header("References")]
        [Header("Player")]
        [SerializeField] private PlayerBody body;
        [SerializeField] private PlayerJump jump;

        [Header("Camera")]
        [SerializeField] private FollowPlayer cameraController;
        [SerializeField] private Transform cameraTransform;

        private Vector3 _desiredDirection;
        private Vector2 _input;

        private float _acceleration;

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
            if (_desiredDirection.magnitude > Mathf.Epsilon && _input.magnitude < Mathf.Epsilon) //TODO: Mathf.Epsilon?
                body.RequestBrake(Model.MovementBreakMultiplier);

            Vector3 movementInput = _input;
            _desiredDirection = TransformDirectionRelativeToCamera(movementInput);
            body.SetMovement(new MovementRequest(_desiredDirection, Model.Speed, _acceleration));
        }

        private Vector3 TransformDirectionRelativeToCamera(Vector2 input)
        {
            Vector3 direction = new Vector3(input.x, 0, input.y); //TODO: Research why z has to be 0

            Vector3 cameraForward = cameraTransform.forward;
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
                cameraController.SetInputRotation(cameraInput);
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
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }

            if (!jump)
            {
                Debug.LogError($"{name}: {nameof(jump)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }

            if (!cameraController)
            {
                Debug.LogError($"{name}: {nameof(cameraController)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }

            if (!cameraTransform)
            {
                Debug.LogError($"{name}: {nameof(cameraTransform)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}