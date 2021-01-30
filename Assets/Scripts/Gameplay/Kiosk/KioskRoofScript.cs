using UnityEngine;

namespace Gameplay.Kiosk
{
    public class KioskRoofScript : MonoBehaviour
    {
        public Collider roofInKiosk;

        public Collider roofInLobby;

        public void Awake()
        {
            roofInKiosk.enabled = false;
            roofInLobby.enabled = false;
        }

        public void TimeToCollide()
        {
            roofInKiosk.enabled = true;
            roofInLobby.enabled = true;
        }
    }
}