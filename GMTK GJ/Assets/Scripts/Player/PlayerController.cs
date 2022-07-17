using UnityEngine;

namespace GMTKGJ
{
    [RequireComponent(typeof(PlayerMotor))]
    [RequireComponent(typeof(PlayerCombat))]
    [RequireComponent(typeof(Interaction))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerMotor m_Motor;
        private PlayerCombat m_Combat;
        private Interaction m_Interaction;

        private float m_Move = 0.0f;
        private float m_MoveY = 0.0f;

        private void Start()
        {
            m_Motor = GetComponent<PlayerMotor>();
            m_Combat = GetComponent<PlayerCombat>();
            m_Interaction = GetComponent<Interaction>();

            InputSystem.Player.Jump.performed += m_Motor.OnJump;
            InputSystem.Player.Jump.performed += m_Motor.OnWallJump;
            InputSystem.Player.Jump.canceled += m_Motor.OnJumpStop;

            InputSystem.Player.Dash.performed += m_Motor.OnDash;

            //InputSystem.Player.Attack.performed += m_Combat.SetAttack;
        }

        private void Update()
        {
            m_Move = InputSystem.Player.Move.ReadValue<float>();
            m_MoveY = InputSystem.Player.MoveY.ReadValue<float>();

            if (Mathf.Abs(m_MoveY) >= 0.5f)
                m_Interaction.Interact();

            m_Motor.Animate(m_Move);
        }

        private void FixedUpdate()
        {
            m_Motor.Move(m_Move);
            m_Combat.MoveY = m_MoveY;
        }
    }
}
