using UnityEngine;
using Events;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        private void OnEnable()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.AudioAction, PlayAudio);
        }

        private void OnDisable()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.AudioAction, PlayAudio);
        }

        private void PlayAudio(params object[] args)
        {
            if (args.Length == 2 && args[0] is AudioEvent audioEvent && args[1] is Vector3 position)
            {
                if (audioEvent == null || audioEvent.clip == null) return;

                GameObject audioObj = new GameObject("AudioSource_" + audioEvent.clip.name);
                audioObj.transform.position = position;
                AudioSource source = audioObj.AddComponent<AudioSource>();

                source.clip = audioEvent.clip;
                source.loop = audioEvent.loop;
                source.playOnAwake = audioEvent.playOnAwake;
                source.volume = audioEvent.volume;
                source.spatialBlend = audioEvent.spatialBlend;

                source.Play();

                //TODO: Destroy when finished playing? mm :/
                if (!audioEvent.loop)
                    Destroy(audioObj, audioEvent.clip.length);
            }
        }
    }
}