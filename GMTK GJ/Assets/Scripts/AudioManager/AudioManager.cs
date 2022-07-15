using UnityEngine;
using System.Collections.Generic;

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

    public static void PlaySound(string name) => s_Instance.PlaySoundImpl(name);
    public static void PlaySound(int index) => s_Instance.PlaySoundImpl(index);

    private void PlaySoundImpl(string name)
	{
        if (!m_SoundsIndex.TryGetValue(name, out int index))
        {
            Debug.LogError($"AudioManager: Sound '{name}' not found!", this);
            return;
        }

        PlaySoundImpl(index);
	}

    private void PlaySoundImpl(int index)
	{
        if (index < 0 || index >= m_Sounds.Length)
        {
            Debug.LogError($"AudioManager: Sound with index ({index}) not found!", this);
            return;
        }

        Sound s = m_Sounds[index];

        if (!s.PlayedConstantly)
            s.Play();
        else
        {
            s.Source = s.GameObject.AddComponent<AudioSource>();

            s.Play();
            Destroy(s.Source, s.Clip.length);
        }
    }
}
