using UnityEngine;

public class Singelton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (s_Instance == null)
            {
                GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
                if (gm)
                    s_Instance = gm.AddComponent<T>();
                else
                {
                    gm = new GameObject("GameManager");
                    gm.tag = "GameManager";
                    s_Instance = gm.AddComponent<T>();
                }
            }

            return s_Instance;
        }
    }

    protected static T s_Instance;
}
