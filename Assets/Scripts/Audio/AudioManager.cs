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
                if (audioEvent == null || audioEvent.clip == null || caller == null) return;

                //TODO: Revise logic --> TryGetValue?
                if (!audioSources.TryGetValue(caller, out AudioSource source))
                {
                    GameObject audioObj = new GameObject("AudioSource_" + caller.name);
                    audioObj.transform.SetParent(caller.transform);
                    audioObj.transform.localPosition = Vector3.zero;
                    source = audioObj.AddComponent<AudioSource>();
                    audioSources[caller] = source;
                }

                //TODO: it might not be necessary to check on all of this each time
                //TODO: what if i need a game object to produce 2 sounds at once? can this happen?

                source.clip = audioEvent.clip;
                source.loop = audioEvent.loop;
                source.playOnAwake = audioEvent.playOnAwake;
                source.volume = audioEvent.volume;
                source.spatialBlend = audioEvent.spatialBlend;

                source.Play();
            }
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