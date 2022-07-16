using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    private static LevelLoader s_Instance;

    [SerializeField] private AnimationClip m_FadeStartAnimation;
    
    private Animator m_Animator;

    private void Awake()
    {
        if (s_Instance == null)
            s_Instance = this;
        else Destroy(gameObject);

        m_Animator = GetComponent<Animator>();

        DontDestroyOnLoad(this);
    }

    public static void LoadLevel(string name) => s_Instance.StartCoroutine(s_Instance.LoadLevelImpl(name));

    private IEnumerator LoadLevelImpl(string name)
    {
        m_Animator.SetTrigger("Start");

        yield return new WaitForSeconds(m_FadeStartAnimation.length);

        SceneManager.LoadScene(name);

        m_Animator.SetTrigger("End");
    }
}
