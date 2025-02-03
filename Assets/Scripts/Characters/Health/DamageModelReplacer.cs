using UnityEngine;

namespace Characters.Health
{
    [CreateAssetMenu(menuName = "Models/Damage/DamageReplacer", fileName = "DM_Replacer")]
    public class DamageModelReplacer : ScriptableObject
    {
        [SerializeField] private DamageModelContainer invincibleModelContainer;
        [SerializeField] private string tagToSearch = "Player"; //TODO: find a better way to do this

        private DamageModelContainer replacement;

        private void OnEnable()
        {
            replacement = invincibleModelContainer;
        }

        public void ReplaceDamageModelContainer()
        {
            var target = GameObject.FindGameObjectWithTag(tagToSearch);

            if (!target) return;

            //TODO: Try get component?? There must be a better way --> Character data source?
            if (target.TryGetComponent(out Character character))
            {
                if (replacement == character.DamageModelContainer)
                {
                    replacement = invincibleModelContainer;
                }

                var temp = character.DamageModelContainer;

                character.DamageModelContainer = replacement;
                replacement = temp;
            }
        }
    }
}