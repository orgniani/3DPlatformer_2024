using UnityEngine;
using Gameplay;
using DataSources;
using TMPro;

namespace UI
{
    public class UIHUD : MonoBehaviour
    {
        [Header("Data Sources")]
        [SerializeField] private DataSource<GameManager> gameManagerDataSource;

        [Header("References")]
        [SerializeField] private TMP_Text levelLabel;

        private GameManager _gameManager;

        private void Awake()
        {
            ValidateReferences();
        }

        private void OnEnable()
        {
            if (gameManagerDataSource.Value != null)
            {
                _gameManager = gameManagerDataSource.Value;
                levelLabel.text = _gameManager.GetLevelLabel();
            }
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
        }

    }
}