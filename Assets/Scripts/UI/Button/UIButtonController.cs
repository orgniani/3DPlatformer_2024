using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public sealed class UIButtonController : MonoBehaviour
    {
        [SerializeField] private TMP_Text buttonText;

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
            buttonText.SetText(config.Text);
            _id = config.ID;
            OnClick = onClick;
        }

        private void HandleButtonClick()
        {
            OnClick?.Invoke(_id);
        }
    }
}
