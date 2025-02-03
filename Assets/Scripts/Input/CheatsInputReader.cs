using Events;
using UnityEngine;
using UnityEngine.InputSystem;
using Player.Brain; //TODO: Check if there's a better way to do this :)
using Characters.Health; //TODO: Check if there's a better way to do this also
using Gameplay; //TODO: Check if there's a better way to do this also

namespace Input
{
    [RequireComponent((typeof(InputReader)))]
    public class CheatsInputReader : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private string actionMapName = "Cheats";

        [Header("Tags")]
        [SerializeField] private string tagToSearch = "EndOfLevel"; //TODO: find a better way to do this

        [Header("Replacers")]
        [SerializeField] private BrainModelReplacer brainModelReplacer;
        [SerializeField] private DamageModelReplacer damageModelReplacer;

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
            var target = GameObject.FindGameObjectWithTag(tagToSearch);
            if (!target) return;

            if (ctx.phase == InputActionPhase.Started)
            {
                if (target.TryGetComponent(out EndOfLevelManager endOfLevel))
                {
                    endOfLevel.InvokeOnWinAction();
                }

                Debug.Log("NEXT LEVEL INPUT");
            }
        }

        private void HandleGodModeInput(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Started)
            {
                damageModelReplacer.ReplaceDamageModelContainer();
                Debug.Log("GOD MODE INPUT");
            }
        }

        private void HandleFlashInput(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Started)
            {
                brainModelReplacer.ReplaceBrainModelContainer();
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

            if (!brainModelReplacer)
            {
                Debug.LogError($"{name}: {nameof(brainModelReplacer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!damageModelReplacer)
            {
                Debug.LogError($"{name}: {nameof(damageModelReplacer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}