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

        public PlayerLocationState whereIs;

        private KioskEntranceSolidCollider kioskSolid;

        private KioskEntranceTriggerCollider kioskTrigger;

        private NPCScript npc;

        private bool _paused;

        private AudioSource m_AudioSource;

        public bool Paused
        {
            get { return _paused; }
        }


        private void Awake()
        {
            deliveryCount = 0;
            thePlayer = FindObjectOfType<PlayerController>();
            pickupParent = FindObjectOfType<PickupParentScript>();
            theDoor = FindObjectOfType<KioskDoor>();
            whereIs = PlayerLocationState.InLobby;

            warpLocations = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlayerWarpPoint"));

            kioskSolid = FindObjectOfType<KioskEntranceSolidCollider>();
            kioskTrigger = FindObjectOfType<KioskEntranceTriggerCollider>();
            
            npc = FindObjectOfType<NPCScript>();

            _paused = false;
            
            m_AudioSource = GetComponent<AudioSource>();
        }

        public void PauseButtonPressed()
        {
            if (_paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            _paused = true;
            Time.timeScale = 0;
            m_AudioSource.Pause();
        }

        public void Unpause()
        {
            _paused = false;
            Time.timeScale = 1;
            m_AudioSource.UnPause();
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

            whereIs = PlayerLocationState.IsLost;
            kioskTrigger.TimeToCollide();
            kioskSolid.StopColliding();
        }

        public void PlayerHasJumped()
        {
            if (whereIs != PlayerLocationState.IsFree)
            {
                whereIs = PlayerLocationState.IsFree;
                kioskSolid.StopColliding();
            }
        }

        public void PlayerHitTheKioskTriggerCollider()
        {
            whereIs = PlayerLocationState.InLobby;
            kioskSolid.TimeToCollide();
            kioskTrigger.StopColliding();
            
            //TODO: cutscene with player talking to npc

            if (deliveryCount == 3)
            {
                //bye bye npc
                npc.ByeByeNPC();
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

    public enum PlayerLocationState
    {
        InLobby,
        IsLost,
        IsFree
    }
}