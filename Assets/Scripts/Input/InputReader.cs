using UnityEngine;
using UnityEngine.InputSystem;
using Events;

namespace Input
{
    public class InputReader : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] protected InputActionAsset inputActions;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _lookAction;
        private InputAction _pauseAction;

        public InputActionAsset InputActions => inputActions;

        private void Awake()
        {
            if (!inputActions)
            {
                Debug.LogError($"{name}: {nameof(inputActions)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            _moveAction = inputActions.FindAction(GameEvents.MoveAction);
            if (_moveAction != null)
            {
                _moveAction.performed += HandleMovementInput;
                _moveAction.canceled += HandleMovementInput;
            }

            _jumpAction = inputActions.FindAction(GameEvents.JumpAction);
            if (_jumpAction != null)
            {
                _jumpAction.started += HandleJumpInput;
                _jumpAction.canceled += HandleJumpInput;
            }

            _lookAction = inputActions.FindAction(GameEvents.LookAction);
            if (_lookAction != null)
            {
                _lookAction.started += HandleCameraInput;
                _lookAction.canceled += HandleCameraInput;
            }

            _pauseAction = inputActions.FindAction(GameEvents.PauseAction);
            if (_pauseAction != null)
            {
                _pauseAction.started += HandlePauseInput;
                _pauseAction.canceled += HandlePauseInput;
            }
        }

        private void OnDisable()
        {
            if (_moveAction != null)
            {
                _moveAction.performed -= HandleMovementInput;
                _moveAction.canceled -= HandleMovementInput;
            }

            if (_jumpAction != null)
            {
                _jumpAction.started -= HandleJumpInput;
                _jumpAction.canceled -= HandleJumpInput;
            }

            if (_lookAction != null)
            {
                _lookAction.started -= HandleCameraInput;
                _lookAction.canceled -= HandleCameraInput;
            }

            if (_pauseAction != null)
            {
                _pauseAction.started -= HandlePauseInput;
                _pauseAction.canceled -= HandlePauseInput;
            }
        }

        private void HandleMovementInput(InputAction.CallbackContext ctx)
        {
            Vector2 movementInput = ctx.ReadValue<Vector2>();

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.MoveAction, movementInput);
        }

        private void HandleJumpInput(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Started)
            {
                if (EventManager<string>.Instance)
                    EventManager<string>.Instance.InvokeEvent(GameEvents.JumpAction, _jumpAction);
            }
        }

        private void HandleCameraInput(InputAction.CallbackContext ctx)
        {
            Vector2 cameraInput = ctx.ReadValue<Vector2>();

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.LookAction, cameraInput);
        }

        private void HandlePauseInput(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Started)
            {
                if (EventManager<string>.Instance)
                    EventManager<string>.Instance.InvokeEvent(GameEvents.PauseAction, _pauseAction);
            }
        }
    }
}