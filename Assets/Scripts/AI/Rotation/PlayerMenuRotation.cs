using System.Collections;
using UnityEngine;

namespace AI.Rotation
{
    public class PlayerMenuRotation : SimpleRotation
    {
        //TODO: Better way to do this?
        //TODO: Validate References!!!
        [Header("References")]
        [SerializeField] private Animator animator;

        [Header("Animator Parameters")]
        [SerializeField] private string waveTriggerParameter = "on_wave";

        protected override IEnumerator Rotate()
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
                    HandleWave();
                }

                float curveValue = Model.RotationCurve.Evaluate(elapsedTime);
                Vector3 rotationAngles = Model.RotationAxis.normalized * curveValue * fullAngleRotation;
                transform.localRotation = _initialRotation * Quaternion.Euler(rotationAngles);

                yield return new WaitForFixedUpdate();
            }
        }

        private void HandleWave()
        {
            animator.SetTrigger(waveTriggerParameter);
        }
    }
}
