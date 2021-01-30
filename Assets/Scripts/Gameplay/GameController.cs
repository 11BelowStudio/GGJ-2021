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

        public AudioClip delivery1;
        public AudioClip delivery2;
        public AudioClip delivery3;

        public AudioClip free;

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
            m_AudioSource.clip = delivery1;
            m_AudioSource.Play();
            m_AudioSource.loop = true;
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

            switch (deliveryCount)
            {
                case 2:
                    m_AudioSource.Stop();
                    m_AudioSource.clip = delivery2;
                    m_AudioSource.loop = true;
                    m_AudioSource.Play();
                    break;
                case 3:
                    m_AudioSource.Stop();
                    m_AudioSource.clip = delivery3;
                    m_AudioSource.loop = true;
                    m_AudioSource.Play();
                    theDoor.gameObject.SetActive(false);
                    break;
            }

            whereIs = PlayerLocationState.IsLost;
            kioskTrigger.TimeToCollide();
            kioskSolid.StopColliding();
        }

        public void PlayerHasJumped()
        {
            if (whereIs != PlayerLocationState.IsFree)
            {
                m_AudioSource.clip = free;
                m_AudioSource.loop = true;
                m_AudioSource.Play();
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
                m_AudioSource.Stop();
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