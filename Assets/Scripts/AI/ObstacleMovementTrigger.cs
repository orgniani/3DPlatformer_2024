using UnityEngine;

namespace AI
{
    public class ObstacleMovementTrigger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ObstacleMovement obstacle;
        [SerializeField] private LayerMask targetLayer;

        private bool _shouldCollide = true;

        private void Awake()
        {
            ValidateReferences();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_shouldCollide) return;

            if (((1 << other.gameObject.layer) & targetLayer.value) != 0)
            {
                StartCoroutine(obstacle.PushForward());

                _shouldCollide = false;
            }

        }

        private void ValidateReferences()
        {
            if (targetLayer == 0)
            {
                Debug.LogError($"{name}: {nameof(targetLayer)} is not set!");
                return;
            }

            if (!obstacle)
            {
                Debug.LogError($"{name}: {nameof(obstacle)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}