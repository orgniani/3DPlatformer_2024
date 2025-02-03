using UnityEngine;

namespace Player.Brain
{
    [CreateAssetMenu(menuName = "Models/PlayerBrainReplacer", fileName = "BRM_Replacer")]
    public class BrainModelReplacer : ScriptableObject
    {
        [SerializeField] private BrainModelContainer flashBrainModelContainer;
        [SerializeField] private string tagToSearch = "Player"; //TODO: find a better way to do this

        private BrainModelContainer replacement;

        private void OnEnable()
        {
            replacement = flashBrainModelContainer;
        }

        public void ReplaceBrainModelContainer()
        {
            var target = GameObject.FindGameObjectWithTag(tagToSearch);

            if (!target) return;

            //TODO: Try get component??
            if (target.TryGetComponent(out PlayerSetup setup))
            {
                if (replacement == setup.BrainModelContainer)
                {
                    replacement = flashBrainModelContainer;
                }

                var temp = setup.BrainModelContainer;

                setup.BrainModelContainer = replacement;
                replacement = temp;
            }
        }
    }
}
