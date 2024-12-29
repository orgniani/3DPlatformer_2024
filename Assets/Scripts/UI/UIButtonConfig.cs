using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "Config/Button", fileName = "ButtonConfig", order = 0)]
    public class UIButtonConfig : ScriptableObject
    {
        [field: SerializeField] public string Label { get; private set; }

        [field: SerializeField] public bool IsExitButton { get; private set; } = false;

    }
}