using UnityEngine;
using UnityEngine.InputSystem;
using Events;
using System.Collections;
using Camera.FollowTarget;
using DataSources;
using Gameplay;

namespace Input
{
    public class InputReader : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] protected InputActionAsset inputActions;

        [Header("Data Sources")]
        [SerializeField] private DataSource<LevelManager> levelManagerDataSource;

        [Header("Replacers")]
        [SerializeField] private FollowPlayerModelReplacer cameraModelReplacer;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _lookAction;
        private InputAction _pauseAction;

        public InputActionAsset InputActions => inputActions;
        public DataSource<LevelManager> LevelManagerDataSource => levelManagerDataSource;

        private Vector2 _controllerCameraInput;
        private bool _isListeningForStickInput = false;
        private bool _isUsingController = false;

        private void Awake()
        {
            ValidateReferences();
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
                _lookAction.performed += HandleCameraInput;
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
                _lookAction.performed -= HandleCameraInput;
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
            if (!levelManagerDataSource.Value) return;

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

        //TODO: Revisit logic --> Handle camera input for CONTROLLER
        private void HandleCameraInput(InputAction.CallbackContext ctx)
        {
            Vector2 cameraInput = ctx.ReadValue<Vector2>();
            bool isGamepad = ctx.control.device is Gamepad;

            if (isGamepad != _isUsingController)
            {
                _isUsingController = isGamepad;
                cameraModelReplacer.ReplaceCameraModelContainer();
            }

            if (isGamepad)
            {
                HandleControllerInput(cameraInput);
                return;
            }

            HandleMouseInput(cameraInput);
        }

        private void HandleMouseInput(Vector2 cameraInput)
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.LookAction, cameraInput);
        }

        private void HandleControllerInput(Vector2 cameraInput)
        {
            if (cameraInput.magnitude <= 0.01f)
            {
                _isListeningForStickInput = false;
                return;
            }

            _controllerCameraInput = cameraInput;

            if (!_isListeningForStickInput)
            {
                _isListeningForStickInput = true;
                StartCoroutine(ProcessControllerLook());
            }
        }

        private IEnumerator ProcessControllerLook()
        {
            while (_isListeningForStickInput)
            {
                if (EventManager<string>.Instance)
                    EventManager<string>.Instance.InvokeEvent(GameEvents.LookAction, _controllerCameraInput);


                if (_controllerCameraInput.magnitude <= 0.01f)
                {
                    _isListeningForStickInput = false;
                    break;
                }


                yield return null;
            }
        }

        private void HandlePauseInput(InputAction.CallbackContext ctx)
        {
            if (!levelManagerDataSource.Value) return;
            if (ctx.phase == InputActionPhase.Started)
            {
                if (EventManager<string>.Instance)
                    EventManager<string>.Instance.InvokeEvent(GameEvents.PauseAction, _pauseAction);
            }
        }

        private void ValidateReferences()
        {
            if (!inputActions)
            {
                Debug.LogError($"{name}: {nameof(inputActions)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!levelManagerDataSource)
            {
                Debug.LogError($"{name}: {nameof(levelManagerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!cameraModelReplacer)
            {
                Debug.LogError($"{name}: {nameof(cameraModelReplacer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}