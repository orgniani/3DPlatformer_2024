using System;
using System.Collections.Generic;
using UnityEngine;
using DataSources;
using Gameplay;
using Events;
using System.Linq;
using Audio;

namespace UI
{
    public class UINavigationManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<GameManager> gameManagerDataSource;

        [Header("Menus")]
        [Tooltip("The first item on this list will be set as the default")]
        [SerializeField] private List<MenuWithId> menusWithId;

        [Header("Buttons")]
        [SerializeField] private List<UIButtonConfig> buttonConfigs = new();

        [Header("Audio")]
        //TODO: Should this be moved? --> CLICK BUTTON AUDIO
        [SerializeField] private AudioEvent clickButtonAudio;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private int _currentMenuIndex = 0;
        private GameManager _gameManager;

        private void Awake()
        {
            ValidateReferences();
        }

        private void OnEnable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.WinAction, HandleOpenWinMenu);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.LoseAction, HandleOpenLoseMenu);
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.PauseAction, HandleOpenPauseMenu);
            }
        }

        private void Start()
        {
            var menuIds = new List<string>();

            foreach (var menu in menusWithId)
            {
                if (menuIds.Contains(menu.ID))
                {
                    if (enableLogs) Debug.LogWarning($"{name}: Menu ID {menu.ID} has already been added! " +
                                                     $"\n Ignoring to avoid issues.");
                    continue;
                }
                menuIds.Add(menu.ID);

                menu.MenuScript.Setup();
                menu.MenuScript.OnChangeMenu += HandleMenuOptions;
                menu.MenuScript.gameObject.SetActive(false);
            }

            menusWithId[_currentMenuIndex].MenuScript.gameObject.SetActive(true);

            if (gameManagerDataSource.Value != null) _gameManager = gameManagerDataSource.Value;
        }

        private void OnDisable()
        {
            if (EventManager<string>.Instance)
            {
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.WinAction, HandleOpenWinMenu);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.LoseAction, HandleOpenLoseMenu);
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.PauseAction, HandleOpenPauseMenu);
            }
        }

        private void HandleOpenWinMenu(params object[] args)
        {
            if (_gameManager && _gameManager.IsFinalLevel)
                HandleMenuOptions(GameEvents.WinAction);
        }

        private void HandleOpenLoseMenu(params object[] args)
        {
            HandleMenuOptions(GameEvents.LoseAction);
        }

        private void HandleOpenPauseMenu(params object[] args)
        {
            if (_gameManager.IsGamePaused)
                HandleMenuOptions(GameEvents.PauseAction);

            else
                menusWithId[_currentMenuIndex].MenuScript.gameObject.SetActive(false);
        }

        private void HandleMenuOptions(string id)
        {
            var buttonConfig = buttonConfigs.FirstOrDefault(config => config.Label == id);

            if (buttonConfig != null && buttonConfig.IsExitButton)
            {
                ExitGame();
                return;
            }

            PlayClickButtonAudio();

            HandleClickPlayButton(buttonConfig);

            HandleClickMenuButton(id);
        }

        private void HandleClickPlayButton(UIButtonConfig buttonConfig)
        {
            if (buttonConfig != null && _gameManager)
            {
                //TODO: Check so that this doesnt replay the TUTORIAL level
                _gameManager.HandlePlayGame();
                menusWithId[_currentMenuIndex].MenuScript.gameObject.SetActive(false);
            }
        }

        private void HandleClickMenuButton(string id)
        {
            for (var i = 0; i < menusWithId.Count; i++)
            {
                var menuWithId = menusWithId[i];

                if (menuWithId.ID == id)
                {
                    if (_currentMenuIndex >= menusWithId.Count)
                    {
                        if (enableLogs) Debug.Log($"{name}: Current menu index {_currentMenuIndex} is out of bounds.");
                        return;
                    }

                    menusWithId[_currentMenuIndex].MenuScript.gameObject.SetActive(false);
                    menuWithId.MenuScript.gameObject.SetActive(true);
                    _currentMenuIndex = i;

                    break;
                }
            }

            HandlePauseMenuButtons(id);
        }

        private void HandlePauseMenuButtons(string id)
        {
            // TODO: Avoid hardcoded names --> The id should probably be a buttonconfig thing
            if (id == "Resume")
            {
                _gameManager.HandlePauseGame();
                menusWithId[_currentMenuIndex].MenuScript.gameObject.SetActive(false);
            }

            // TODO: Avoid hardcoded names --> The id should probably be a buttonconfig thing
            else if (id == "Main Menu" && _gameManager.IsGamePaused)
            {
                _gameManager.OnGameOver();
                _gameManager.HandlePauseGame(); // Unpause the game

                //TODO: This gets repeated too many times --> Cursor logic
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        //TODO: Should this be moved? --> PLAY CICK BUTTON AUDIO
        private void PlayClickButtonAudio()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, clickButtonAudio, gameObject);
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        [Serializable]
        public struct MenuWithId
        {
            [field: SerializeField] public string ID { get; set; }
            [field: SerializeField] public UIMenu MenuScript { get; set; }
        }

        private void ValidateReferences()
        {
            if (!gameManagerDataSource)
            {
                Debug.LogError($"{name}: {nameof(gameManagerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            foreach (var menu in menusWithId)
            {
                if (menu.MenuScript == null)
                {
                    Debug.LogError($"{name}: a {nameof(menu.MenuScript)} is null!" +
                                   $"\nDisabling component to avoid errors.");
                    enabled = false;
                }
            }

            if (menusWithId.Count <= 0)
            {
                Debug.LogError($"{name}: the list of {nameof(menusWithId)} is empty!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            foreach (var button in buttonConfigs)
            {
                if (button == null)
                {
                    Debug.LogError($"{name}: a {nameof(button)} is null!" +
                                   $"\nDisabling component to avoid errors.");
                    enabled = false;
                }
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