using UnityEngine;

namespace Gameplay.Kiosk
{
    public class KioskDoor : MonoBehaviour
    {
        private GameController gc;

        private void Awake()
        {
            gc = GameObject.FindObjectOfType<GameController>();
        }
        
        public void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                gc.PlayerWalkedIntoDoor();
            }
        }
    }
}