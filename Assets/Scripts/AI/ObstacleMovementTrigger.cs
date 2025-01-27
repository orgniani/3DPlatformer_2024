using UnityEngine;

namespace AI
{
    public class ObstacleMovementTrigger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ObstacleMovement[] obstacles;
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
                foreach(var obstacle in obstacles)
                {
                    StartCoroutine(obstacle.PushForward());
                }

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

            if (obstacles.Length <= 0)
            {
                Debug.LogError($"{name}: {nameof(obstacles)} array is empty!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}