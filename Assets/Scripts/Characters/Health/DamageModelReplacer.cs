using DataSources;
using UnityEngine;

namespace Characters.Health
{
    [CreateAssetMenu(menuName = "Models/Damage/DamageReplacer", fileName = "DM_Replacer")]
    public class DamageModelReplacer : ScriptableObject
    {
        [SerializeField] private DamageModelContainer invincibleModelContainer;
        [SerializeField] private DataSource<Character> characterDataSource;

        private DamageModelContainer _replacement;
        private Character _target;

        private void OnEnable()
        {
            if (!AreReferencesValidated()) return;

            _replacement = invincibleModelContainer;

            if (characterDataSource.Value != null)
                _target = characterDataSource.Value;
        }

        public void ReplaceDamageModelContainer()
        {
            if (!_target) return;

            if (_replacement == _target.DamageModelContainer)
            {
                _replacement = invincibleModelContainer;
            }

            var temp = _target.DamageModelContainer;

            _target.DamageModelContainer = _replacement;
            _replacement = temp;
        }

        private bool AreReferencesValidated()
        {
            if (!invincibleModelContainer)
            {
                Debug.LogError($"{name}: {nameof(invincibleModelContainer)} is null!");
                return false;
            }

            if (!characterDataSource)
            {
                Debug.LogError($"{name}: {nameof(characterDataSource)} is null!");
                return false;
            }

            return true;
        }
    }
}