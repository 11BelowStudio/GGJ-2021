using UnityEngine;

namespace Gameplay.Kiosk
{
    public class KioskEntranceTriggerCollider : MonoBehaviour
    {
        private GameController gc;

        private Collider theCollider;

        public void Awake()
        {
            gc = GameObject.FindObjectOfType<GameController>();
            theCollider = GetComponent<Collider>();
            theCollider.enabled = false;
        }

        public void TimeToCollide()
        {
            theCollider.enabled = true;
        }

        public void StopColliding()
        {
            theCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                gc.PlayerHitTheKioskTriggerCollider();
            }
        }
    }
}