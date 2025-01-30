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

        [Header("3D SOUND SETTING")]
        [Range(0f, 1f)] public float spatialBlend = 0f;

        public float minDistance = 10f;
        public float maxDistance = 300f;
    }
}
