using UnityEngine;

namespace AI.Trampoline
{
    [RequireComponent(typeof(Trampoline))]
    public class TrampolineSetup : MonoBehaviour
    {
        [SerializeField] private TrampolineModelContainer trampolineModelContainer;
        private Trampoline _trampoline;

        private void Awake()
        {
            _trampoline = GetComponent<Trampoline>();
            ValidateAndAssignValues();
        }

        private void ValidateAndAssignValues()
        {
            if (_trampoline && trampolineModelContainer)
            {
                _trampoline.Model = trampolineModelContainer.Model;
                _trampoline.enabled = true;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(_trampoline)} or {nameof(trampolineModelContainer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}