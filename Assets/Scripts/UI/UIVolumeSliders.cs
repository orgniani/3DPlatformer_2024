using Audio;
using DataSources;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIVolumeSliders : MonoBehaviour
    {
        [Header("Data Sources")]
        [SerializeField] private DataSource<AudioManager> audioManagerDataSource;

        [Header("Sliders")]
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        private string _masterVolumeKey;
        private string _musicVolumeKey;
        private string _sfxVolumeKey;

        private List<Slider> _sliders = new();
        private AudioManager _audioManager;

        private void Awake()
        {
            ValidateReferences();
        }

        public void Setup()
        {
            _sliders = new List<Slider> { masterSlider, musicSlider, sfxSlider };
        }

        public List<Selectable> GetSliders()
        {
            return _sliders.Cast<Selectable>().ToList();
        }

        private IEnumerator Start()
        {
            while (_audioManager == null)
            {
                if (audioManagerDataSource.Value != null)
                    _audioManager = audioManagerDataSource.Value;

                yield return null;
            }

            _masterVolumeKey = _audioManager.MasteVolumeKey;
            _musicVolumeKey = _audioManager.MusicVolumeKey;
            _sfxVolumeKey = _audioManager.SFXVolumeKey;

            InitiateSliders();
        }

        private void InitiateSliders()
        {
            masterSlider.value = PlayerPrefs.GetFloat(_masterVolumeKey, 1);
            musicSlider.value = PlayerPrefs.GetFloat(_musicVolumeKey, 1);
            sfxSlider.value = PlayerPrefs.GetFloat(_sfxVolumeKey, 1);

            masterSlider.onValueChanged.AddListener(UpdateMasterVolume);
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        }

        private void UpdateMasterVolume(float value)
        {
            _audioManager.SetMasterVolume(value);
            PlayerPrefs.SetFloat(_masterVolumeKey, value);
        }

        private void UpdateMusicVolume(float value)
        {
            _audioManager.SetMusicVolume(value);
            PlayerPrefs.SetFloat(_musicVolumeKey, value);
        }

        private void UpdateSFXVolume(float value)
        {
            _audioManager.SetSFXVolume(value);
            PlayerPrefs.SetFloat(_sfxVolumeKey, value);
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

            if (!masterSlider)
            {
                Debug.LogError($"{name}: {nameof(masterSlider)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!musicSlider)
            {
                Debug.LogError($"{name}: {nameof(musicSlider)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!sfxSlider)
            {
                Debug.LogError($"{name}: {nameof(sfxSlider)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}
