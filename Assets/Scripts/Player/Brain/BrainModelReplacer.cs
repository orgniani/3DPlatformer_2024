using DataSources;
using UnityEngine;
using Player.Setup;

namespace Player.Brain
{
    [CreateAssetMenu(menuName = "Models/Player/Brain/BrainReplacer", fileName = "BRM_Replacer")]
    public class BrainModelReplacer : ScriptableObject
    {
        [SerializeField] private BrainModelContainer flashBrainModelContainer;
        [SerializeField] private DataSource<PlayerSetup> playerDataSource;

        private BrainModelContainer replacement;
        private PlayerSetup _player;

        private void OnEnable()
        {
            if (!AreReferencesValidated()) return;

            replacement = flashBrainModelContainer;

            if (playerDataSource.Value != null)
                _player = playerDataSource.Value;
        }

        public void ReplaceBrainModelContainer()
        {
            if (!_player) return;

            if (replacement == _player.BrainModelContainer)
            {
                replacement = flashBrainModelContainer;
            }

            var temp = _player.BrainModelContainer;

            _player.BrainModelContainer = replacement;
            replacement = temp;
        }

        private bool AreReferencesValidated()
        {
            if (!flashBrainModelContainer)
            {
                Debug.LogError($"{name}: {nameof(flashBrainModelContainer)} is null!");
                return false;
            }

            if (!playerDataSource)
            {
                Debug.LogError($"{name}: {nameof(playerDataSource)} is null!");
                return false;
            }

            return true;
        }
    }
}
