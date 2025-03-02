using UnityEngine;
using Events;
using System.Collections.Generic;
using DataSources;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Data Sources")]
        [SerializeField] private DataSource<AudioManager> audioManagerDataSource;

        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer mainMixer;
        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private AudioMixerGroup sfxGroup;

        [Header("Audio Configs")]
        [SerializeField] private AudioConfig mainMusic;
        [SerializeField] private AudioConfig loseAudio;
        [SerializeField] private AudioConfig winAudio;

        private string masterVolume = "MasterVolume";
        private string musicVolume = "MusicVolume";
        private string sfxVolume = "SFXVolume";

        private Dictionary<GameObject, Dictionary<AudioClip, AudioSource>> audioSources = new();

        public string MasteVolume => masterVolume;
        public string MusicVolume => musicVolume;
        public string SFXVolume => sfxVolume;

        private void Awake()
        {
            ValidateReferences();
        }

        private void OnEnable()
        {
            audioManagerDataSource.Value = this;

            SetAudioVolume();
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
            if (audioManagerDataSource != null && audioManagerDataSource.Value == this)
                audioManagerDataSource.Value = null;

            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.PlayAudioAction, PlayAudio);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.WinAction, PlayWinAudio);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.LoseAction, PlayLoseAudio);
            }
        }

        private void SetAudioVolume()
        {
            float masterVol = PlayerPrefs.GetFloat(masterVolume, 1);
            float musicVol = PlayerPrefs.GetFloat(musicVolume, 1);
            float sfxVol = PlayerPrefs.GetFloat(sfxVolume, 1);

            SetMasterVolume(masterVol);
            SetMusicVolume(musicVol);
            SetSFXVolume(sfxVol);
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

            newSource.outputAudioMixerGroup = audioConfig.IsMusic ? musicGroup : sfxGroup;

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

        public void SetMasterVolume(float volume) => mainMixer.SetFloat(masterVolume, ConvertToDecibels(volume));
        public void SetMusicVolume(float volume) => mainMixer.SetFloat(musicVolume, ConvertToDecibels(volume));
        public void SetSFXVolume(float volume) => mainMixer.SetFloat(sfxVolume, ConvertToDecibels(volume));

        private float ConvertToDecibels(float volume)
        {
            return volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
        }

        private void ValidateReferences()
        {
            if (!audioManagerDataSource)
            {
                Debug.LogError($"{name}: {nameof(audioManagerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!mainMixer)
            {
                Debug.LogError($"{name}: {nameof(mainMixer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!musicGroup)
            {
                Debug.LogError($"{name}: {nameof(musicGroup)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!sfxGroup)
            {
                Debug.LogError($"{name}: {nameof(sfxGroup)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!mainMusic)
            {
                Debug.LogError($"{name}: {nameof(mainMusic)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!loseAudio)
            {
                Debug.LogError($"{name}: {nameof(loseAudio)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!winAudio)
            {
                Debug.LogError($"{name}: {nameof(winAudio)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}