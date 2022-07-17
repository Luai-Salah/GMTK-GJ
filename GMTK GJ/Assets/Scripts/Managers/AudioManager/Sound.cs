using UnityEngine;

namespace GMTKGJ {
    [System.Serializable]
    public class Sound
    {
        public string Name;

        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume = 0.7f;
        [Range(0f, .5f)]
        public float RandomVolume = 0.0f;

        [Range(0.1f, 3f)]
        public float Pitch = 1.0f;
        [Range(0f, .5f)]
        public float RandomPitch = 0.0f;

        public bool Loop = false;
        public bool PlayedConstantly = false;

        [HideInInspector] public GameObject GameObject;

        public AudioSource Source
        {
            get { return m_Source; }
            set
            {
                m_Source = value;
                m_Source.clip = Clip;
            }
        }

        private AudioSource m_Source;

        public void Play(float time = 0.0f)
        {
            m_Source.volume = Volume * (1 + Random.Range(-RandomVolume / 2f, RandomVolume / 2f));
            m_Source.pitch = Pitch * (1 + Random.Range(-RandomPitch / 2f, RandomPitch / 2f));
            m_Source.loop = Loop;
            m_Source.time = time;
            m_Source.Play();
        }
    }
}
