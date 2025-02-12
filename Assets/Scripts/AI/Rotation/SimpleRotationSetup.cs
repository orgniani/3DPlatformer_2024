using UnityEngine;

namespace AI.Rotation
{
    [RequireComponent(typeof(SimpleRotation))]
    public class SimpleRotationSetup : MonoBehaviour
    {
        [SerializeField] private SimpleRotationModelContainer simpleRotationModelContainer;
        private SimpleRotation _simpleRotation;

        private void Awake()
        {
            _simpleRotation = GetComponent<SimpleRotation>();
            ValidateAndAssignValues();
        }

        private void ValidateAndAssignValues()
        {
            if (simpleRotationModelContainer)
            {
                _simpleRotation.Model = simpleRotationModelContainer.Model;
                _simpleRotation.enabled = true;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(simpleRotationModelContainer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}