using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gameplay.ButtonPickup;
using Gameplay.Player;
using Random = UnityEngine.Random;

namespace Gameplay
{
    
    
    public class GameController : MonoBehaviour
    {
        
        

        public PlayerController thePlayer;

        public PickupParentScript pickupParent;

       

        private void Awake()
        {
            thePlayer = FindObjectOfType<PlayerController>();
            pickupParent = FindObjectOfType<PickupParentScript>();
        }


        public void MakePickupPlayNoise(ControlPickupEnum noisemaker)
        {
            pickupParent.MakePickupPlayNoise(noisemaker);
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