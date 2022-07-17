using UnityEngine;

namespace GMTKGJ
{
    public class Interaction : MonoBehaviour
    {
        private DialogueTrigger m_NPCDialogueTrigger;
        private DialogueTrigger m_CollectableDialogueTrigger;

        private Player m_Player;

        private void Start() => m_Player = GetComponent<Player>();

        public void Interact()
        {
            if (m_NPCDialogueTrigger)
                m_NPCDialogueTrigger.TriggerDialogue();
            else if (m_CollectableDialogueTrigger)
            {
                m_CollectableDialogueTrigger.TriggerDialogue();
                m_Player.HandleCollectable(m_CollectableDialogueTrigger.GetComponent<Collectable>());
                AudioManager.PlaySound("Collect");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("NPC"))
            {
                m_NPCDialogueTrigger = collision.transform.GetComponent<DialogueTrigger>();
                m_CollectableDialogueTrigger = null;
            }
            else if (collision.CompareTag("Collectable"))
            {
                m_CollectableDialogueTrigger = collision.transform.GetComponent<DialogueTrigger>();
                m_NPCDialogueTrigger = null;
            }
        }
    }
}
