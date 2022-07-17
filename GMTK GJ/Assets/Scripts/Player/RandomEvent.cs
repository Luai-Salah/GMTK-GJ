using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        [SerializeField] private Sprite[] m_DiceFaces;

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
        private float m_OJF = 0.0f;

        // Anger
        private float m_GroundTimer = 0.0f;

        private bool m_CoolDown = false;
        private int m_Event = 0;
        private float m_EffectTimer = 0.0f;
        private float m_CoolDownTimer = 0.0f;

        private PlayerMotor m_Motor;
        private Player m_Player;
        private Animator m_VolumeAnimator;
        private Animator m_DiceAnimator;
        private Image m_DiceImage;
        private TextMeshProUGUI m_NextRollText;
        private TextMeshProUGUI m_EventText;

        private void Start()
        {
            m_Motor = GetComponent<PlayerMotor>();
            m_Player = GetComponent<Player>();
            m_VolumeAnimator = GameObject.FindGameObjectWithTag("Volume").GetComponent<Animator>();

            Transform dice = GameObject.FindGameObjectWithTag("Dice").transform;
            m_DiceAnimator = dice.GetComponentInChildren<Animator>();
            m_DiceImage = dice.GetComponentInChildren<Image>();
            m_NextRollText = dice.transform.Find("Timer").GetComponentInChildren<TextMeshProUGUI>();
            m_EventText = dice.transform.Find("Event").GetComponentInChildren<TextMeshProUGUI>();

            EventCoolDown();
        }

        private void EventCoolDown()
        {
            m_CoolDown = true;

            switch (m_Event)
            {
                case 1:
                    Time.timeScale = 1.0f;
                    MusicManager.StopMusic("Attrition");
                    break;
                case 2:
                    m_VolumeAnimator.SetTrigger("Out");
                    MusicManager.StopMusic("Sorrow");
                    break;
                case 3:
                    MusicManager.StopMusic("Anger");
                    break;
                case 4:
                    m_Motor.GravityScale = m_OGS;
                    m_Motor.FallGravityMultiplier = m_OFGM;
                    m_Motor.JumpForce = m_OJF;
                    MusicManager.StopMusic("Anxiety");
                    break;
                case 5:
                    m_Motor.GravityScale = m_OGS;
                    m_Motor.FallGravityMultiplier = m_OFGM;
                    m_Motor.JumpForce = m_OJF;
                    MusicManager.StopMusic("Nastalogy");
                    break;
                case 6:
                    Time.timeScale = 1.0f;
                    MusicManager.StopMusic("Panic");
                    break;
                default:
                    break;
            }

            Invoke("Roll", m_EventCoolDown);
            m_CoolDownTimer = m_EventCoolDown;

            m_EventText.color = Color.white;
            m_EventText.text = "None";
        }

        private void Roll()
        {
            m_CoolDown = false;
            m_Event = Random.Range(1, 6);

            m_DiceAnimator.SetTrigger("Roll");
            m_DiceImage.sprite = m_DiceFaces[m_Event - 1];
            AudioManager.PlaySound("Dice");

            Invoke("EventCoolDown", m_EventDuration);

            switch (m_Event)
            {
                case 1:
                    m_EventText.text = "Attrition";
                    m_EventText.color = Color.cyan;
                    Time.timeScale = 0.5f;
                    MusicManager.UpdateMusic("Attrition");
                    break;
                case 2:
                    m_VolumeAnimator.SetTrigger("In");
                    m_EventText.text = "Sorrow";
                    m_EventText.color = Color.gray;
                    MusicManager.UpdateMusic("Sorrow");
                    break;
                case 3:
                    m_EventText.text = "Anger";
                    m_EventText.color = Color.red;
                    m_GroundTimer = m_TimeAllowedOnGround;
                    MusicManager.UpdateMusic("Anger");
                    break;
                case 4:
                    m_EventText.text = "Anxiety";
                    m_EventText.color = Color.black;
                    m_OGS = m_Motor.GravityScale;
                    m_OFGM = m_Motor.FallGravityMultiplier;
                    m_OJF = m_Motor.JumpForce;
                    m_Motor.GravityScale *= 2f;
                    m_Motor.FallGravityMultiplier *= 2f;
                    m_Motor.JumpForce *= 2f;
                    MusicManager.UpdateMusic("Anxiety");
                    break;
                case 5:
                    m_OGS = m_Motor.GravityScale;
                    m_OFGM = m_Motor.FallGravityMultiplier;
                    m_OJF = m_Motor.JumpForce;
                    m_Motor.GravityScale = m_GravityScale;
                    m_Motor.FallGravityMultiplier = m_FallGravityMultiplier;
                    m_Motor.JumpForce /= 2f;
                    m_EventText.text = "Nastalogy";
                    m_EventText.color = Color.blue;
                    MusicManager.UpdateMusic("Nastalogy");
                    break;
                case 6:
                    Time.timeScale = m_GameSpeed;
                    m_EventText.text = "Panic";
                    m_EventText.color = Color.yellow;
                    MusicManager.UpdateMusic("Panic");
                    break;
                default:
                    break;
            }

            m_EffectTimer = m_EventDuration;
        }

        private void StopMomentom()
        {
            for (int i = 0; i < 30; i++)
                m_Motor.Stop();
        }

        private void Update()
        {
            if (CoolDown)
            {
                m_CoolDownTimer -= Time.deltaTime;
                if (m_CoolDownTimer <= 0.0f)
                    m_CoolDownTimer = 0.0f;

                m_NextRollText.text = $"Next Roll:{m_CoolDownTimer.ToString("0")}";
            }
            else
            {
                if (m_Event == 3)
                {
                    if (m_Motor.IsGrounded)
                        m_GroundTimer -= Time.deltaTime;
                    else m_GroundTimer = m_TimeAllowedOnGround;

                    if (m_GroundTimer <= 0.0f)
                        m_Player.TakeDamage(1);
                }

                m_EffectTimer -= Time.deltaTime;
                if (m_EffectTimer <= 0.0f)
                    m_EffectTimer = 0.0f;

                m_NextRollText.text = $"Cool Down:{m_EffectTimer.ToString("0")}";
            }
        }
    }
}
