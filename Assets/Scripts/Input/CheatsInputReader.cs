using Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    [RequireComponent((typeof(InputReader)))]
    public class CheatsInputReader : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private string actionMapName = "Cheats";

        private InputReader _inputReader;

        private InputActionAsset _inputActions;
        private InputActionMap _cheatsActionMap;

        private InputAction _nextLevelAction;
        private InputAction _godModeAction;
        private InputAction _flashAction;

        private void Awake()
        {
            _inputReader = GetComponent<InputReader>();
            _inputActions = _inputReader.InputActions;

            _cheatsActionMap = _inputActions.FindActionMap(actionMapName, true);
            ValidateReferences();
        }

        private void OnEnable()
        {
            _cheatsActionMap?.Enable();

            _nextLevelAction = _inputActions.FindAction(GameEvents.NextLevelAction);
            if (_nextLevelAction != null)
            {
                _nextLevelAction.started += HandleNextLevelInput;
                _nextLevelAction.canceled += HandleNextLevelInput;
            }

            _godModeAction = _inputActions.FindAction(GameEvents.GodModeAction);
            if (_godModeAction != null)
            {
                _godModeAction.started += HandleGodModeInput;
                _godModeAction.canceled += HandleGodModeInput;
            }

            _flashAction = _inputActions.FindAction(GameEvents.FlashAction);
            if (_flashAction != null)
            {
                _flashAction.started += HandleFlashInput;
                _flashAction.canceled += HandleFlashInput;
            }
        }

        private void OnDisable()
        {
            if (_nextLevelAction != null)
            {
                _nextLevelAction.started -= HandleNextLevelInput;
                _nextLevelAction.canceled -= HandleNextLevelInput;
            }

            if (_godModeAction != null)
            {
                _godModeAction.started -= HandleGodModeInput;
                _godModeAction.canceled -= HandleGodModeInput;
            }

            if (_flashAction != null)
            {
                _flashAction.started -= HandleFlashInput;
                _flashAction.canceled -= HandleFlashInput;
            }
        }

        private void HandleNextLevelInput(InputAction.CallbackContext ctx)
        {
            //Debug.Log("NEXT LEVEL INPUT");

            if (ctx.phase == InputActionPhase.Started)
            {
                Debug.Log("NEXT LEVEL INPUT");
            }
        }

        private void HandleGodModeInput(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Started)
            {
                Debug.Log("GOD MODE INPUT");
            }
        }

        private void HandleFlashInput(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Started)
            {
                Debug.Log("FLASH INPUT");
            }
        }

        private void ValidateReferences()
        {
            if (!_inputActions)
            {
                Debug.LogError($"{name}: {nameof(_inputActions)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (_cheatsActionMap == null)
            {
                Debug.LogError($"{name}: {nameof(_cheatsActionMap)} not found!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}