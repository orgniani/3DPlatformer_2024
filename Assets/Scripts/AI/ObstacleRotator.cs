using UnityEngine;

namespace AI
{
    public class ObstacleRotator : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float speedMultiplier = 1f;
        [SerializeField] private Vector3 rotationAxis = Vector3.up;

        [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private float elapsedTime = 0f;
        private Quaternion initialRotation;

        private void Start()
        { 
            initialRotation = transform.localRotation;
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime * speedMultiplier;

            float curveValue = rotationCurve.Evaluate(elapsedTime % 1f);
            Vector3 rotationAngles = rotationAxis.normalized * curveValue * 360f;

            transform.localRotation = initialRotation * Quaternion.Euler(rotationAngles.x, rotationAngles.y, rotationAngles.z);
        }
    }
}