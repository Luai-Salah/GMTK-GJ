using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTKGJ
{
    public static class InputSystem
    {
        private static InputActions s_Instance;

        public static InputActions.PlayerActions Player { get { return s_Player; } }
        private static InputActions.PlayerActions s_Player;

        public static InputActions.DialogueActions Dialogue { get { return s_Dialogue; } }
        private static InputActions.DialogueActions s_Dialogue;

        public static void Init()
        {
            s_Instance = new InputActions();

            s_Player = s_Instance.Player;
            s_Dialogue = s_Instance.Dialogue;

            s_Instance.Player.Enable();
        }

        public static void Shutdown()
        {
            s_Instance.Disable();
        }
    }
}
