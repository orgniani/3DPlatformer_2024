using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "Config/Audio", fileName = "AudioCfg", order = 0)]
    public class AudioEvent : ScriptableObject
    {
        public AudioClip clip;
        public bool loop = false;
        public bool playOnAwake = false;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0f, 1f)] public float spatialBlend = 1f;
    }
}
