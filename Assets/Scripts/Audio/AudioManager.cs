using UnityEngine;
using Events;
using System.Collections.Generic;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        private Dictionary<GameObject, AudioSource> audioSources = new Dictionary<GameObject, AudioSource>();
        [SerializeField] private AudioEvent loseAudio;
        [SerializeField] private AudioEvent winAudio;

        private void OnEnable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.PlayAudioAction, PlayAudio);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.WinAction, PlayWinAudio);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.LoseAction, PlayLoseAudio);
            }
        }

        private void OnDisable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.PlayAudioAction, PlayAudio);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.WinAction, PlayWinAudio);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.LoseAction, PlayLoseAudio);
            }
        }

        private void PlayAudio(params object[] args)
        {
            if (args.Length == 2 && args[0] is AudioEvent audioEvent && args[1] is GameObject caller)
            {
                if (audioEvent == null || audioEvent.Clip == null || caller == null) return;

                //TODO: Revise logic --> TryGetValue?
                if (!audioSources.TryGetValue(caller, out AudioSource source))
                    source = CreateAudioSource(caller, audioEvent, source);

                source.clip = audioEvent.Clip;
                source.loop = audioEvent.Loop;
                source.volume = audioEvent.Volume;

                source.spatialBlend = audioEvent.SpatialBlend;
                source.maxDistance = audioEvent.MaxDistance;
                source.minDistance = audioEvent.MinDistance;

                source.Play();
            }
        }

        private AudioSource CreateAudioSource(GameObject caller, AudioEvent audioEvent, AudioSource source)
        {
            GameObject audioObj = new GameObject("AudioSource_" + caller.name);
            audioObj.transform.SetParent(caller.transform);
            audioObj.transform.localPosition = Vector3.zero;
            source = audioObj.AddComponent<AudioSource>();
            audioSources[caller] = source;

            return source;
        }

        private void PlayWinAudio(params object[] args)
        {
            PlayAudio(winAudio, gameObject);
        }

        private void PlayLoseAudio(params object[] args)
        {
            PlayAudio(loseAudio, gameObject);
        }
    }
}