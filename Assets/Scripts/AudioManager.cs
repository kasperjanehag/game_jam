    using System;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [System.Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip Clip;

        private AudioSource m_audioSource;

        [Range(0f, 1f)] public float Volume = 0.7f;
        [Range(0.5f, 1.5f)] public float Pitch = 1f;

        [Range(0f, 0.5f)] public float RandomVolume = 0.1f;
        [Range(0f, 0.5f)] public float RandomPitch = 1f;

        public bool IsLooping;

        public void SetSource(AudioSource source)
        {
            m_audioSource = source;
            source.clip = Clip;
            source.loop = IsLooping;
        }

        public void Play()
        {
            m_audioSource.volume = Volume * (1 + Random.Range(-RandomVolume / 2f, RandomVolume / 2f));
            m_audioSource.pitch = Pitch * (1 + Random.Range(-RandomPitch / 2f, RandomPitch / 2f));
            m_audioSource.Play();
        }
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField]
        private Sound[] m_sounds;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (Instance != null)
            {
                throw new Exception("More than one AudioManager in the scene.");
            }

            Instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < m_sounds.Length; ++i)
            {
                GameObject go = new GameObject($"Sound_{i}_{m_sounds[i].Name}");
                go.transform.parent = this.transform;
                m_sounds[i].SetSource(go.AddComponent<AudioSource>());
            }

        PlaySound("GameLoop");
        }

        public void PlaySound(string name)
        {
            foreach (var sound in m_sounds)
            {
                if (sound.Name == name)
                {
                    sound.Play();
                    return;
                }
            }
            Debug.LogWarning($"[AudioManager] No sound found with name {name}");
        }
    }
