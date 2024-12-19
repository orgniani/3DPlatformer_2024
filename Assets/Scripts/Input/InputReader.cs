using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Input
{
    public class InputReader : MonoBehaviour
    {
        //TODO: These should be handled by the event manager
        public event Action<Vector2> onMovementInput = delegate { };
        public event Action<Vector2> onCameraInput = delegate { };
        public event Action onJumpInput = delegate { };

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
