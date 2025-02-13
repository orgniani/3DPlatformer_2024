using UnityEngine;
using Events;
using System.Collections.Generic;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        private Dictionary<GameObject, Dictionary<AudioClip, AudioSource>> audioSources = new();

        [SerializeField] private AudioConfig mainMusic;

        [SerializeField] private AudioConfig loseAudio;
        [SerializeField] private AudioConfig winAudio;

        private void OnEnable()
        {
            PlayAudio(mainMusic, gameObject);

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
            if (args.Length == 2 && args[0] is AudioConfig audioConfig && args[1] is GameObject caller)
            {
                if (audioConfig == null || audioConfig.Clip == null || caller == null) return;

                AudioSource source = GetOrCreateAudioSource(caller, audioConfig);
                
                source.Play();
            }
        }

        private AudioSource GetOrCreateAudioSource(GameObject caller, AudioConfig audioConfig)
        {
            if (!audioSources.TryGetValue(caller, out var clipMap))
            {
                clipMap = new Dictionary<AudioClip, AudioSource>();
                audioSources[caller] = clipMap;
            }

            if (clipMap.TryGetValue(audioConfig.Clip, out AudioSource existingSource))
                return existingSource;

            AudioSource newSource = CreateNewAudioSource(caller, audioConfig);
            clipMap[audioConfig.Clip] = newSource;

            return newSource;
        }

        private AudioSource CreateNewAudioSource(GameObject caller, AudioConfig audioConfig)
        {
            GameObject audioObj = new GameObject($"AudioSource_{caller.name}_{audioConfig.Clip.name}");
            audioObj.transform.SetParent(caller.transform);
            audioObj.transform.localPosition = Vector3.zero;

            AudioSource newSource = audioObj.AddComponent<AudioSource>();

            newSource.clip = audioConfig.Clip;
            newSource.loop = audioConfig.Loop;
            newSource.volume = audioConfig.Volume;

            newSource.spatialBlend = audioConfig.SpatialBlend;
            newSource.maxDistance = audioConfig.MaxDistance;
            newSource.minDistance = audioConfig.MinDistance;

            return newSource;
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