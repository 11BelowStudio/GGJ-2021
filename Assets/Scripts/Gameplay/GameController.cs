using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gameplay.ButtonPickup;
using Gameplay.Kiosk;
using Gameplay.Player;
using Random = UnityEngine.Random;

namespace Gameplay
{
    
    
    public class GameController : MonoBehaviour
    {
        
        

        public PlayerController thePlayer;

        public PickupParentScript pickupParent;

        public int deliveryCount;

        private List<GameObject> warpLocations;

        private KioskDoor theDoor;

       

        private void Awake()
        {
            deliveryCount = 0;
            thePlayer = FindObjectOfType<PlayerController>();
            pickupParent = FindObjectOfType<PickupParentScript>();
            theDoor = FindObjectOfType<KioskDoor>();

            warpLocations = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlayerWarpPoint"));
        }


        public void MakePickupPlayNoise(ControlPickupEnum noisemaker)
        {
            pickupParent.MakePickupPlayNoise(noisemaker);
        }


        public void ThePlayerGotLost()
        {
            deliveryCount++;
            pickupParent.HidePickups(thePlayer.LoseControls(deliveryCount));
            
            thePlayer.Teleport(warpLocations[Random.Range(0,warpLocations.Count)].transform.position);

            if (deliveryCount == 3)
            {
                theDoor.gameObject.SetActive(false);
            }
        }

    }
    
    public enum ControlPickupEnum{
        Forward,
        Backward,
        Left,
        Right,
        Jump
    }
}