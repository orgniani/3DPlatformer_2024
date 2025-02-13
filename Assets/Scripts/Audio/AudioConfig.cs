using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "Config/Audio", fileName = "AudioCfg", order = 0)]
    public class AudioConfig : ScriptableObject
    {
        [field: SerializeField] public AudioClip Clip { get; private set; }

        [field: SerializeField] public bool Loop { get; private set; }

        [field: SerializeField, Range(0f, 1f)] public float Volume { get; private set; } = 1f;

        [field: SerializeField, Range(0f, 1f)] public float SpatialBlend { get; private set; } = 0f;

        [field: SerializeField] public float MinDistance { get; private set; } = 10f;

        [field: SerializeField] public float MaxDistance { get; private set; } = 300f;
    }
}
