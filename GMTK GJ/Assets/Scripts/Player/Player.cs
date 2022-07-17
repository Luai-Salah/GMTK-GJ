using System;
using UnityEngine;

namespace GMTKGJ
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int m_MaxHealth = 3;
        [SerializeField] private LayerMask m_DamageLayer;

        private int m_CurHealth;
        private Vector3 m_CheckPoint;

        private PlayerMotor m_Motor;

        private void Start()
        {
            m_CurHealth = m_MaxHealth;
            m_Motor = GetComponent<PlayerMotor>();
        }

        public void TakeDamage(int damage)
        {
            m_CurHealth -= damage;

            if (m_CurHealth <= 0)
            {
                m_CurHealth = 0;
                Die();
            } else AudioManager.PlaySound("Hurt");

            transform.localPosition = m_CheckPoint;
        }

        public void HandleCollectable(Collectable collectable)
        {
            switch (collectable.CollectableType)
            {
                case Collectables.WallJump:
                    m_Motor.CollectWallJump = true;
                    break;
                case Collectables.Dash:
                    m_Motor.CollectDash = true;
                    break;
                default:
                    break;
            }

            Destroy(collectable.gameObject);
        }

        private void Die()
        {
            AudioManager.PlaySound("Death");
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Damage"))
                TakeDamage(1);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("CheckPoint"))
                m_CheckPoint = new Vector3(collision.transform.position.x, collision.transform.position.y);
        }
    }
}
