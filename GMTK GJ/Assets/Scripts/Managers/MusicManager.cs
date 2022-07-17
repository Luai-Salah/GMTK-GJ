using UnityEngine;

namespace GMTKGJ
{
    public class MusicManager : Singelton<MusicManager>
    {
        [SerializeField] private string m_Base;

        private Sound m_BaseSound;
        private Sound m_CurSound;
        private float m_Volume;

        private bool m_Stop;

        private void Awake()
        {
            if (!s_Instance)
                s_Instance = this;
            else Destroy(this);
        }

        private void Start()
        {
            m_BaseSound = AudioManager.PlaySound(m_Base);
        }

        public static void UpdateMusic(string musicName)
        {
            Instance.m_Stop = false;
            Instance.m_CurSound = AudioManager.PlaySound(musicName, Instance.m_BaseSound.Source.time);
            Instance.m_Volume = Instance.m_CurSound.Source.volume;
            Instance.m_CurSound.Source.volume = 0.0f;
        }

        public static void StopMusic(string musicName)
        {
            Instance.m_Stop = true;
            AudioManager.StopSound(musicName);
            Instance.m_CurSound = null;
        }

        private void Update()
        {
            if (m_CurSound != null)
            {
                if (m_CurSound.Source.volume != m_Volume)
                    m_CurSound.Source.volume = Mathf.Lerp(m_CurSound.Volume, m_Volume, Time.deltaTime);
            }
        }
    }
}
