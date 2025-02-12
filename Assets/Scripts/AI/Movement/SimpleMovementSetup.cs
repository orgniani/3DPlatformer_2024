using UnityEngine;

namespace AI.Movement
{
    [RequireComponent(typeof(SimpleMovementController))]
    public class SimpleMovementSetup : MonoBehaviour
    {
        [SerializeField] private SimpleMovementModelContainer simpleMovementModelContainer;
        private SimpleMovementController _simpleMovement;

        private void Awake()
        {
            _simpleMovement = GetComponent<SimpleMovementController>();
            ValidateAndAssignValues();
        }

        private void ValidateAndAssignValues()
        {
            if (simpleMovementModelContainer)
            {
                _simpleMovement.Model = simpleMovementModelContainer.Model;
                _simpleMovement.enabled = true;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(simpleMovementModelContainer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}
