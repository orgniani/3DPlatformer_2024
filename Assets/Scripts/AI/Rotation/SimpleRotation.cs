using UnityEngine;
using Events;
using System.Collections;

namespace AI.Rotation
{
    public class SimpleRotation : MonoBehaviour
    {
        private Quaternion _initialRotation;
        private int _lastCycle = 0;
        private float _fullRotationAngle = 360;

        private Coroutine _rotationCoroutine;

        public SimpleRotationModel Model { get; set; }

        private void Start()
        { 
            _initialRotation = transform.localRotation;
            _rotationCoroutine = StartCoroutine(Rotate());

            if (Model.RotationAudio)
            {
                if (Model.RotationAudio.Loop) PlayRotationSound();
            }
        }

        private void OnDisable()
        {
            if (_rotationCoroutine != null)
                StopCoroutine(_rotationCoroutine);
        }

        private IEnumerator Rotate()
        {
            float elapsedTime = 0f;

            while (enabled)
            {
                elapsedTime += Time.deltaTime * Model.Speed;

                //TODO: Get rid of nested IFS!
                if (Model.RotationAudio)
                {
                    if (!Model.RotationAudio.Loop)
                    {
                        int currentCycle = Mathf.FloorToInt(elapsedTime);

                        if (currentCycle > _lastCycle)
                        {
                            _lastCycle = currentCycle;
                            PlayRotationSound();
                        }
                    }
                }

                float curveValue = Model.RotationCurve.Evaluate(elapsedTime);
                Vector3 rotationAngles = Model.RotationAxis.normalized * curveValue * _fullRotationAngle;
                transform.localRotation = _initialRotation * Quaternion.Euler(rotationAngles);

                yield return new WaitForFixedUpdate();
            }
        }

        private void PlayRotationSound()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, Model.RotationAudio, gameObject);
        }
    }
}