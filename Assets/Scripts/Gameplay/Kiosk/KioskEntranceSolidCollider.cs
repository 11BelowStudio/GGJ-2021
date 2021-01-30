using System;
using UnityEngine;

namespace Gameplay.Kiosk
{
    public class KioskEntranceSolidCollider : MonoBehaviour
    {
        private GameController gc;

        private Collider theCollider;

        public void Awake()
        {
            gc = GameObject.FindObjectOfType<GameController>();
            theCollider = GetComponent<Collider>();
            theCollider.enabled = true;
        }

        public void TimeToCollide()
        {
            theCollider.enabled = true;
        }

        public void StopColliding()
        {
            theCollider.enabled = false;
        }
        
    }
}