using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTKGJ
{
    public class RoomLoader : MonoBehaviour
    {
        [SerializeField] private string m_RoomName;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                LevelLoader.LoadLevel(m_RoomName);
        }
    }
}
