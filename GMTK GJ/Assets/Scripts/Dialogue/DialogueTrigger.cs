using UnityEngine;

namespace GMTKGJ
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private Dialogue m_Dialogue;

        private DialogueManager m_DialogueManager;

        private void Start() => m_DialogueManager = DialogueManager.Instance;

        public void TriggerDialogue()
        {
            m_DialogueManager.StartDialogue(m_Dialogue);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                TriggerDialogue();
        }
    }
}
