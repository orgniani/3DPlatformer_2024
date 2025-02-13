using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Audio;
using Events;
using UnityEngine.EventSystems;

namespace UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public sealed class UIButtonController : MonoBehaviour, IPointerEnterHandler, ISelectHandler
    {
        [SerializeField] private TMP_Text buttonText;

        [SerializeField] private AudioConfig clickAudio;
        [SerializeField] private AudioConfig hoverAudio;

        private string _id;
        private Button _button;

        public event Action<string> OnClick;

        private void Reset()
        {
            GameObject child;

            if (transform.childCount < 1)
            {
                child = new GameObject("Text (TMP)");
                child.transform.SetParent(transform);
            }

            else child = transform.GetChild(0).gameObject;

            if (!child.TryGetComponent<TMP_Text>(out buttonText))
            {
                buttonText = child.AddComponent<TextMeshProUGUI>();
            }

            _button = GetComponent<Button>();
        }

        private void Awake()
        {
            buttonText ??= GetComponent<TMP_Text>();
            _button ??= GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        public void Setup(UIButtonConfig config, Action<string> onClick)
        {
            if (!buttonText)
            {
                Debug.LogError($"{name}: {nameof(buttonText)} is null in the button prefab!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            buttonText.SetText(config.Label);
            _id = config.Label;
            OnClick = onClick;
        }

        private void HandleButtonClick()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, clickAudio, gameObject);

            OnClick?.Invoke(_id);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayHoverSound();
        }

        public void OnSelect(BaseEventData eventData)
        {
            //TODO: Find a way to get this to work in controller too!
            //PlayHoverSound();
        }

        private void PlayHoverSound()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, hoverAudio, gameObject);
        }
    }
}
