using UnityEngine;
using System.Collections.Generic;

namespace GMTKGJ
{
    public class AudioManager : Singelton<AudioManager>
    {
        [SerializeField] private Sound[] m_Sounds;

        private Dictionary<string, int> m_SoundsIndex = new Dictionary<string, int>();

        private void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if (s_Instance != this)
                Destroy(gameObject);

            for (int i = 0; i < m_Sounds.Length; i++)
            {
                GameObject go = new GameObject($"Sound_{i}_{m_Sounds[i].Name}");
                go.transform.SetParent(transform);
                if (!m_Sounds[i].PlayedConstantly)
                    m_Sounds[i].Source = go.AddComponent<AudioSource>();

                m_Sounds[i].GameObject = go;

                m_SoundsIndex[m_Sounds[i].Name] = i;
            }
        }

        public static Sound PlaySound(string name, float time = 0.0f) { return Instance.PlaySoundImpl(name, time); }
        public static Sound PlaySound(int index, float time = 0.0f) { return Instance.PlaySoundImpl(index, time); }

        public static void StopSound(string name) => Instance.StopSoundImpl(name);
        public static void StopSound(int index) => Instance.StopSoundImpl(index);

        private Sound PlaySoundImpl(string name, float time)
        {
            if (!m_SoundsIndex.TryGetValue(name, out int index))
            {
                Debug.LogError($"AudioManager: Sound '{name}' not found!", this);
                return null;
            }

            return PlaySoundImpl(index, time);
        }

        private Sound PlaySoundImpl(int index, float time)
        {
            if (index < 0 || index >= m_Sounds.Length)
            {
                Debug.LogError($"AudioManager: Sound with index ({index}) not found!", this);
                return null;
            }

            Sound s = m_Sounds[index];

            if (!s.PlayedConstantly)
                s.Play(time);
            else
            {
                s.Source = s.GameObject.AddComponent<AudioSource>();

                s.Play(time);
                Destroy(s.Source, s.Clip.length);
            }

            return s;
        }

        private void StopSoundImpl(string name)
        {
            if (!m_SoundsIndex.TryGetValue(name, out int index))
            {
                Debug.LogError($"AudioManager: Sound '{name}' not found!", this);
                return;
            }

            StopSoundImpl(index);
        }

        private void StopSoundImpl(int index)
        {
            m_Sounds[index].Source.Stop();
        }
    }
}
