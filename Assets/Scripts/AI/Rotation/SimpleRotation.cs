using UnityEngine;
using Events;
using System.Collections;
using Audio;

namespace AI.Rotation
{
    public class SimpleRotation : MonoBehaviour
    {
        private Quaternion _initialRotation;
        private AudioEvent _audio;
        private Coroutine _rotationCoroutine;

        public SimpleRotationModel Model { get; set; }

        private void Awake()
        {
            _audio = Model.RotationAudio;
        }

        private void OnEnable()
        { 
            _initialRotation = transform.localRotation;
            _rotationCoroutine = StartCoroutine(Rotate());

            if (_audio && _audio.Loop) PlayRotationSound();
        }

        private void OnDisable()
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

                if (_audio && !_audio.Loop)
                {
                    int currentCycle = Mathf.FloorToInt(elapsedTime * Model.AudioPlaysPerCycle);

                    if (currentCycle > lastCycle)
                    {
                        lastCycle = currentCycle;
                        PlayRotationSound();
                    }
                }

                float curveValue = Model.RotationCurve.Evaluate(elapsedTime);
                Vector3 rotationAngles = Model.RotationAxis.normalized * curveValue * fullAngleRotation;
                transform.localRotation = _initialRotation * Quaternion.Euler(rotationAngles);

                yield return new WaitForFixedUpdate();
            }
        }

        private void PlayRotationSound()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, _audio, gameObject);
        }
    }
}