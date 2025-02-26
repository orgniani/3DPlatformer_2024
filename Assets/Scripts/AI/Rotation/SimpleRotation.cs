using UnityEngine;
using System.Collections;

namespace AI.Rotation
{
    public class SimpleRotation : MonoBehaviour
    {
        private Quaternion _initialRotation;
        private Coroutine _rotationCoroutine;
        public SimpleRotationModel Model { get; set; }

        protected virtual void Awake()
        {
            _initialRotation = transform.localRotation;
        }

        protected virtual void OnEnable()
        {
            transform.localRotation = _initialRotation;
            _rotationCoroutine = StartCoroutine(Rotate());
        }

        protected virtual void OnDisable()
        {
            if (_rotationCoroutine != null)
                StopCoroutine(_rotationCoroutine);
        }

        private IEnumerator Rotate()
        {
            float elapsedTime = 0f;
            int lastCycle = 0;
            int fullAngleRotation = 360;

            while (enabled)
            {
                elapsedTime += Time.deltaTime * Model.Speed;

                int currentCycle = Mathf.FloorToInt(elapsedTime * Model.AudioPlaysPerCycle);

                if (currentCycle > lastCycle)
                {
                    lastCycle = currentCycle;
                    CompleteRotationAction();
                }

                float curveValue = Model.RotationCurve.Evaluate(elapsedTime);
                Vector3 rotationAngles = Model.RotationAxis.normalized * curveValue * fullAngleRotation;
                transform.localRotation = _initialRotation * Quaternion.Euler(rotationAngles);

                yield return new WaitForFixedUpdate();
            }
        }

        protected virtual void CompleteRotationAction() { }
    }
}