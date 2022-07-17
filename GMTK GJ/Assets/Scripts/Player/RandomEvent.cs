using UnityEngine;
using UnityEngine.Rendering;

// 1 - attrition : vertical deadly rays of light spawning randomly
// 2 - sorrow : blurry screen/mineraft nausea ? 
// 3 - anger : can 't stay in place for too long, cna't stay grounded for too long
// 4 - anxiety : regularly entirely stop momentum or motion briefly
// 5 - nostalgy : makes the player very floaty by reducing both jump speed and fall speed
// 6 - panic : general acceleration of the game 

namespace GMTKGJ
{
    public class RandomEvent : MonoBehaviour
    {
        public bool CoolDown { get { return m_CoolDown; } }
        public int Event { get { return m_Event; } }

        [SerializeField] private float m_EventDuration = 15f;
        [SerializeField] private float m_EventCoolDown = 15f;

        [Header("Anger")]
        [SerializeField] private float m_TimeAllowedOnGround = 1.0f;

        [Header("Anxiety")]
        [SerializeField] private float m_TimeBetweenStops = 4.0f;

        [Header("Nostalgy")]
        [SerializeField] private float m_GravityScale = 1.0f;
        [SerializeField] private float m_FallGravityMultiplier = 2.0f;

        [Header("Panic")]
        [SerializeField] private float m_GameSpeed = 2.0f;

        // Nostalgy
        private float m_OGS = 0.0f;
        private float m_OFGM = 0.0f;

        private bool m_CoolDown = false;
        private int m_Event = 0;
        private int m_LastEvent = 0;

        private PlayerMotor m_Motor;
        private Animator m_VolumeAnimator;

        private void Start()
        {
            m_Motor = GetComponent<PlayerMotor>();
            m_VolumeAnimator = GameObject.FindGameObjectWithTag("Volume").GetComponent<Animator>();
            EventCoolDown();
        }

        private void EventCoolDown()
        {
            m_CoolDown = true;

            switch (m_Event)
            {
                case 2:
                    m_VolumeAnimator.SetTrigger("Out");
                    break;
                case 4:
                    CancelInvoke("StopMomentom");
                    break;
                case 5:
                    m_Motor.GravityScale = m_OGS;
                    m_Motor.FallGravityMultiplier = m_OFGM;
                    break;
                case 6:
                    Time.timeScale = 1.0f;
                    break;
                default:
                    break;
            }

            Invoke("Roll", m_EventCoolDown);
        }

        private void Roll()
        {
            m_CoolDown = false;
            m_Event = Random.Range(1, 6);

            m_LastEvent = m_Event;

            Invoke("EventCoolDown", m_EventDuration);

            switch (m_Event)
            {
                case 2:
                    m_VolumeAnimator.SetTrigger("In");
                    break;
                case 4:
                    InvokeRepeating("StopMomentom", m_TimeBetweenStops, m_TimeBetweenStops);
                    break;
                case 5:
                    m_OGS = m_Motor.GravityScale;
                    m_OFGM = m_Motor.FallGravityMultiplier;
                    m_Motor.GravityScale = m_GravityScale;
                    m_Motor.FallGravityMultiplier = m_FallGravityMultiplier;
                    break;
                case 6:
                    Time.timeScale = m_GameSpeed;
                    break;
                default:
                    break;
            }
        }

        private void StopMomentom()
        {
            for (int i = 0; i < 5; i++)
                m_Motor.Stop();
        }
    }
}
