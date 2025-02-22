using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataSources;
using Gameplay;
using Events;
using System.Linq;
using Audio;
using UI.Buttons;
using Input;

namespace UI
{
    public class UINavigationManager : MonoBehaviour
    {
        //TODO: Revisit script --> UINAVIGATIONMANAGER
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<GameManager> gameManagerDataSource;

        [Header("Systems")]
        [SerializeField] private EventSystem eventSystem;

        [Header("Menus")]
        [Tooltip("The first item on this list will be set as the default")]
        [SerializeField] private List<MenuWithId> menusWithId;

        [Header("Buttons")]
        [SerializeField] private List<UIButtonConfig> buttonConfigs = new();

        [Header("Audio")]
        [SerializeField] private AudioConfig clickButtonAudio;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private int _currentMenuIndex = 0;
        private GameManager _gameManager;

        private Dictionary<UIButtonAction, Action> _buttonActions;

        private void Awake()
        {
            ValidateReferences();
            InitializeButtonActions();
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
            InitializeMenus();

            if (gameManagerDataSource.Value != null) 
                _gameManager = gameManagerDataSource.Value;
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

        private void InitializeButtonActions()
        {
            _buttonActions = new Dictionary<UIButtonAction, Action>
        {
            { UIButtonAction.Resume, ResumeGame },
            { UIButtonAction.Close, CloseMenu },
            { UIButtonAction.Quit, QuitGame },
            { UIButtonAction.Play, PlayGame },
            { UIButtonAction.Restart, RestartGame },
            { UIButtonAction.Exit, ExitGame }
            };
        }

        private void InitializeMenus()
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

                menu.MenuScript.Setup(eventSystem);
                menu.MenuScript.OnChangeMenu += HandleMenuNavigation;
                menu.MenuScript.gameObject.SetActive(false);
            }

            menusWithId[_currentMenuIndex].MenuScript.gameObject.SetActive(true);
        }

        private void HandleOpenWinMenu(params object[] args)
        {
            if (_gameManager && _gameManager.IsFinalLevel)
            {
                OpenMenu(GameEvents.WinAction);
                UnlockCursor();
            }
        }

        private void HandleOpenLoseMenu(params object[] args)
        {
            OpenMenu(GameEvents.LoseAction);
            UnlockCursor();
        }

        private void HandleOpenPauseMenu(params object[] args)
        {
            PlayClickButtonAudio();

            if (_gameManager.IsGamePaused)
            {
                OpenMenu(GameEvents.PauseAction);
                UnlockCursor();
            }

            else
            {
                menusWithId[_currentMenuIndex].MenuScript.gameObject.SetActive(false);
                LockCursor();
            }
        }

        private void HandleMenuNavigation(string id)
        {
            var buttonConfig = buttonConfigs.FirstOrDefault(config => config.Label == id);
            if (buttonConfig != null && _buttonActions.TryGetValue(buttonConfig.Action, out var action))
            {
                if(buttonConfig.Action == UIButtonAction.Exit)
                {
                    ExitGame();
                    return;
                }

                menusWithId[_currentMenuIndex].MenuScript.gameObject.SetActive(false);
                action?.Invoke();
            }

            else OpenMenu(id);

            PlayClickButtonAudio();
        }

        private void OpenMenu(string id)
        {
            var menu = menusWithId.FirstOrDefault(menu => menu.ID == id);
            if (menu.MenuScript == null) return;

            menusWithId[_currentMenuIndex].MenuScript.gameObject.SetActive(false);

            menu.MenuScript.gameObject.SetActive(true);
            _currentMenuIndex = menusWithId.IndexOf(menu);
        }

        private void ResumeGame()
        {
            LockCursor();
            _gameManager.HandlePauseGame();
        }

        private void CloseMenu()
        {
            LockCursor();
        }

        private void QuitGame()
        {
            _gameManager.OnGameOver();
            _gameManager.HandlePauseGame();

            _currentMenuIndex = 0;
            menusWithId[_currentMenuIndex].MenuScript.gameObject.SetActive(true);
        }

        private void PlayGame()
        {
            _gameManager.HandlePlayGame();

            var tutorialMenu = menusWithId.FirstOrDefault(menu => menu.IsTutorialMenu);
            if (tutorialMenu.MenuScript != null)
            {
                _currentMenuIndex = menusWithId.IndexOf(tutorialMenu);
                tutorialMenu.MenuScript.gameObject.SetActive(true);
            }
        }

        private void RestartGame()
        {
            LockCursor();
            _gameManager.HandleRestartLevel();
        }

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

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        [Serializable]
        public struct MenuWithId
        {
            [field: SerializeField] public string ID { get; set; }
            [field: SerializeField] public UIMenu MenuScript { get; set; }
            [field: SerializeField] public bool IsTutorialMenu { get; set; }
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

            if (!eventSystem)
            {
                Debug.LogError($"{name}: {nameof(eventSystem)} is null!" +
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