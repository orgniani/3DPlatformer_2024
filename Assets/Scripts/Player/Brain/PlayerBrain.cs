using Player.Body;
using Player.Jump;
using Camera.FollowTarget;
using Input;
using DataSources;
using UnityEngine;
using System.Collections;

namespace Player.Brain
{
    public class PlayerBrain : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<InputReader> inputReaderDataSource;

        [Header("Player")]
        [SerializeField] private PlayerBody body;
        [SerializeField] private PlayerJump jump;

        [Header("Camera")]
        [SerializeField] private FollowPlayer cameraController;
        [SerializeField] private Transform cameraTransform;

        private InputReader inputReader;

        private Vector3 desiredDirection;
        private Vector2 input;

        private float acceleration;

        public BrainModel Model { get; set; }

        public float Acceleration
        {
            set { acceleration = value; }
        }

        private void Awake()
        {
            ValidateReferences();
        }

        private IEnumerator Start()
        {
            while (inputReader == null)
            {
                //TODO: Get reference to player controller from ReferenceManager/DataSource | DONE

                if (inputReaderDataSource.Value != null)
                    inputReader = inputReaderDataSource.Value;

                yield return null;
            }

            inputReader.onMovementInput += HandleMovementInput;
            inputReader.onJumpInput += HandleJumpInput;
            inputReader.onCameraInput += HandleCameraInput;
        }

        private void OnDisable()
        {
            //TODO: This should be handled by event manager
            inputReader.onMovementInput -= HandleMovementInput;
            inputReader.onJumpInput -= HandleJumpInput;
            inputReader.onCameraInput -= HandleCameraInput;
        }

        public Vector3 GetDesiredDirection() => desiredDirection;

        private void Update()
        {
            if (desiredDirection.magnitude > Mathf.Epsilon && input.magnitude < Mathf.Epsilon) //TODO: Mathf.Epsilon?
                body.RequestBrake(Model.MovementBreakMultiplier);

            Vector3 movementInput = input;
            desiredDirection = TransformDirectionRelativeToCamera(movementInput);
            body.SetMovement(new MovementRequest(desiredDirection, Model.Speed, acceleration));
        }

        private Vector3 TransformDirectionRelativeToCamera(Vector2 input)
        {
            Vector3 direction = new Vector3(input.x, 0, input.y); //TODO: Research why z has to be 0

            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;

            direction = Quaternion.LookRotation(cameraForward) * direction;

            return direction.normalized;
        }

        private void HandleMovementInput(Vector2 input)
        {
            this.input = input;
        }

        private void HandleCameraInput(Vector2 input)
        {
            cameraController.SetInputRotation(input);
        }

        private void HandleJumpInput()
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
