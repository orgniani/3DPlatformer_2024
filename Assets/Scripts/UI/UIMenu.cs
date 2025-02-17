using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UI.Buttons;
using Input;

namespace UI
{
    public class UIMenu : MonoBehaviour
    {
        [Header("References")]
        [Header("Buttons")]
        [SerializeField] private UIButtonController buttonPrefab;
        [SerializeField] private Transform buttonParent;
        [SerializeField] private List<UIButtonConfig> buttonConfigs = new();

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private List<string> _addedButtonLabels = new List<string>();

        private EventSystem _eventSystem;
        private GameObject _firstButton = null;

        public event Action<string> OnChangeMenu;

        private void Awake()
        {
            ValidateReferences();
        }

        private void OnEnable()
        {
            if (_firstButton)
                StartCoroutine(SetSelectedButton());
        }

        private IEnumerator SetSelectedButton()
        {
            yield return null;
            _eventSystem.SetSelectedGameObject(_firstButton);
        }

        public void Setup(EventSystem eventSystem)
        {
            _eventSystem = eventSystem;

            foreach (var config in buttonConfigs)
            {
                if (config == null)
                {
                    if (enableLogs) Debug.LogError($"{name}: a button is null!" +
                                                   $"\n Ignoring to avoid issues.");
                    continue;
                }

                else if (_addedButtonLabels.Contains(config.Label))
                {
                    if (enableLogs) Debug.LogWarning($"{name}: Button with label {config.Label} has already been added to the menu! " +
                                                     $"\n Ignoring to avoid issues.");
                    continue;
                }

                var newButton = Instantiate(buttonPrefab, buttonParent);
                newButton.name = $"{config.Label}_Btn";
                newButton.Setup(config, HandleButtonClick);
                _addedButtonLabels.Add(config.Label);

                if (!_firstButton) _firstButton = newButton.gameObject;
            }
        }

        public GameObject GetFirstSelectableButton()
        {
            return _firstButton;
        }

        private void HandleButtonClick(string id)
        {
            OnChangeMenu?.Invoke(id);
        }

        private void ValidateReferences()
        {
            if (!buttonPrefab)
            {
                Debug.LogError($"{name}: {nameof(buttonPrefab)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!buttonParent)
            {
                Debug.LogError($"{name}: {nameof(buttonParent)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (buttonConfigs.Count <= 0)
            {
                Debug.LogError($"{name}: the list of {nameof(buttonConfigs)} is empty!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}