using UnityEngine;
using Events;
using Audio;

namespace AI
{
    public class ObstacleRotator : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float speedMultiplier = 1f;
        [SerializeField] private Vector3 rotationAxis = Vector3.up;

        [SerializeField] private AudioEvent rotationAudio;

        [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private float _elapsedTime = 0f;
        private Quaternion _initialRotation;
        private int _lastCycle = 0;

        private void Start()
        { 
            _initialRotation = transform.localRotation;
        }

        //TODO: Change UPDATE to COROUTINE
        private void Update()
        {
            _elapsedTime += Time.deltaTime * speedMultiplier;

            //TODO: Revisit logic
            int currentCycle = Mathf.FloorToInt(_elapsedTime);

            if (currentCycle > _lastCycle)
            {
                _lastCycle = currentCycle;
                if (EventManager<string>.Instance)
                    EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, rotationAudio, gameObject);
            }

            float curveValue = rotationCurve.Evaluate(_elapsedTime % 1f);
            Vector3 rotationAngles = rotationAxis.normalized * curveValue * 360f;
            transform.localRotation = _initialRotation * Quaternion.Euler(rotationAngles);
        }
    }
}