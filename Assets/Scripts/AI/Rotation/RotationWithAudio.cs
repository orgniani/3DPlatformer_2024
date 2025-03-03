using AI.Rotation;
using Audio;
using Events;
using UnityEngine;

namespace AI
{
    public class RotationWithAudio : SimpleRotation
    {
        private AudioConfig _audio;

        protected override void OnEnable()
        {
            base.OnEnable();

            _audio = Model.RotationAudio;

            if (!_audio)
            {
                Debug.LogError($"{name}: {nameof(_audio)} is null!" +
                               $"\nDisabling component _audio avoid errors.");
                enabled = false;
                return;
            }

            if (_audio.Loop) PlayRotationSound();
        }

        protected override void CompleteRotationAction()
        {
            if (_audio.Loop) return;
            PlayRotationSound();
        }

        private void PlayRotationSound()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, _audio, gameObject);
        }
    }
}
