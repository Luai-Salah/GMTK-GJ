using UnityEngine;

namespace GMTKGJ
{
    public enum Collectables
    {
        WallJump,
        Dash
    }

    public class Collectable : MonoBehaviour
    {
        public Collectables CollectableType;
    }
}
