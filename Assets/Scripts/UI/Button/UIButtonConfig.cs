using UnityEngine;

namespace UI.Buttons
{
    [CreateAssetMenu(menuName = "Config/Button", fileName = "BtnCfg", order = 0)]
    public class UIButtonConfig : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }

        [field: SerializeField] public string Text { get; private set; }

        [field: SerializeField] public UIButtonAction Action { get; set; }
    }
}