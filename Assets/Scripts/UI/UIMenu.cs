using System;
using System.Collections.Generic;
using UnityEngine;

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

        public event Action<string> OnChangeMenu;

        private void Awake()
        {
            ValidateReferences();
        }

        public void Setup()
        {
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
            }
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