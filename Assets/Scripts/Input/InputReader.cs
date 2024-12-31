using DataSources;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Input
{
    public class InputReader : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<InputReader> inputReaderDataSource;

        //TODO: These should be handled by the event manager
        public event Action<Vector2> onMovementInput = delegate { };
        public event Action<Vector2> onCameraInput = delegate { };
        public event Action onJumpInput = delegate { };

        private void Awake()
        {
            if (!inputReaderDataSource)
            {
                Debug.LogError($"{name}: {nameof(inputReaderDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            inputReaderDataSource.Value = this;
        }

        private void OnDisable()
        {
            if (inputReaderDataSource != null && inputReaderDataSource.Value == this)
                inputReaderDataSource.Value = null;
        }

        public void HandleMovementInput(InputAction.CallbackContext ctx)
        {
            onMovementInput?.Invoke(ctx.ReadValue<Vector2>());
        }

        public void HandleJumpInput(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                onJumpInput?.Invoke();
        }

        public void HandleCameraInput(InputAction.CallbackContext ctx)
        {
            onCameraInput?.Invoke(ctx.ReadValue<Vector2>());
        }

    }
}
