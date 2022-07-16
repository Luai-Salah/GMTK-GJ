using UnityEngine;

namespace GMTKGJ
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : Singelton<GameManager>
    {
        private void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else Destroy(this);

            DontDestroyOnLoad(this);

            InputSystem.Init();
        }

        private void OnDisable()
        {
            InputSystem.Shutdown();
        }
    }
}
