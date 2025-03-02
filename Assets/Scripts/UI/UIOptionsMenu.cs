using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace UI
{
    [RequireComponent(typeof(UIVolumeSliders))]
    public class UIOptionsMenu : UIMenu
    {
        //TODO: Revisit script --> UIOptionsMenu
        private UIVolumeSliders _volumeSliders;
        private List<Selectable> _allInteractables = new();

        public override void Setup(EventSystem eventSystem)
        {
            base.Setup(eventSystem);

            _volumeSliders = GetComponent<UIVolumeSliders>();

            if (!_volumeSliders) return;

            _volumeSliders.gameObject.SetActive(true);
            _volumeSliders.Setup();

            _allInteractables.AddRange(_menuButtons.Select(b => b.GetComponent<Selectable>()));
            _allInteractables.AddRange(_volumeSliders.GetSliders());

            SetCustomNavigation();
        }

        protected override void SetCustomNavigation()
        {
            for (int i = 0; i < _allInteractables.Count; i++)
            {
                var current = _allInteractables[i];
                var navigation = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnUp = (i > 0) ? _allInteractables[i - 1] : null,
                    selectOnDown = (i < _allInteractables.Count - 1) ? _allInteractables[i + 1] : null
                };

                current.navigation = navigation;
            }
        }
    }
}
