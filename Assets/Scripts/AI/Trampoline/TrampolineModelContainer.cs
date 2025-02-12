using UnityEngine;

namespace AI.Trampoline
{
    [CreateAssetMenu(menuName = "Models/AI/Trampoline", fileName = "TM_")]
    public class TrampolineModelContainer : ScriptableObject
    {
        [field: SerializeField] public TrampolineModel Model { get; private set; }
    }
}