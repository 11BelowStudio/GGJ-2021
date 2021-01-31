using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gameplay.ButtonPickup;
using Gameplay.Kiosk;
using Gameplay.Player;
using Utilities.fader;
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

        private KioskRoofScript kioskRoof;

        private NPCScript npc;

        private bool _paused;

        private AudioSource m_AudioSource;

        public AudioClip delivery1;
        public AudioClip delivery2;
        public AudioClip delivery3;

        public AudioClip free;

        private bool theEagleHasLandedOnTheRoof;

        private bool done;

        private PauseMenuScript pauseMenu;

        private DialogueScript dialogue;

        private bool _inDialogue;

        public bool Paused
        {
            get { return (_paused || _inDialogue); }
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
            kioskRoof = FindObjectOfType<KioskRoofScript>();

            npc = FindObjectOfType<NPCScript>();

            _paused = false;

            m_AudioSource = GetComponent<AudioSource>();
            m_AudioSource.clip = delivery1;
            m_AudioSource.Play();
            m_AudioSource.loop = true;

            theEagleHasLandedOnTheRoof = false;

            done = false;

            pauseMenu = FindObjectOfType<PauseMenuScript>();
            dialogue = FindObjectOfType<DialogueScript>();

            _inDialogue = false;
        }

        public void Start()
        {
            _inDialogue = true;
            dialogue.StartConversation(DialogueEnum.First);
        }

        public void PauseButtonPressed()
        {
            if (!_inDialogue)
            {
                if (_paused)
                {
                    Unpause();
                }
                else if (!done)
                {
                    Pause();
                }
            }
        }

        public void Pause()
        {
            _paused = true;
            Time.timeScale = 0;
            m_AudioSource.Pause();
            thePlayer.ForceMouseUnlock();
            pauseMenu.ShowPauseMenu();
        }

        public void Unpause()
        {
            _paused = false;
            Time.timeScale = 1;
            m_AudioSource.UnPause();
            thePlayer.ForceMouseLock();
            pauseMenu.HidePauseMenu();
        }


        public void MakePickupPlayNoise(ControlPickupEnum noisemaker)
        {
            pickupParent.MakePickupPlayNoise(noisemaker);
        }


        public void PlayerWalkedIntoDoor()
        {
            deliveryCount++;

            if (deliveryCount == 4)
            {
                theDoor.gameObject.SetActive(false);
            }
            else
            {
                
                pickupParent.HidePickups(thePlayer.LoseControls(deliveryCount));

                thePlayer.Teleport(warpLocations[Random.Range(0, warpLocations.Count)].transform.position);

                switch (deliveryCount)
                {
                    case 1:
                        thePlayer.ChangeBag(BagEnum.Bag1);
                        break;
                    case 2:
                        thePlayer.ChangeBag(BagEnum.Bag2);
                        m_AudioSource.Stop();
                        m_AudioSource.clip = delivery2;
                        m_AudioSource.loop = true;
                        m_AudioSource.Play();
                        break;
                    case 3:
                        thePlayer.ChangeBag(BagEnum.Bag3);
                        m_AudioSource.Stop();
                        m_AudioSource.clip = delivery3;
                        m_AudioSource.loop = true;
                        m_AudioSource.Play();
                        break;
                }

                whereIs = PlayerLocationState.IsLost;
                kioskTrigger.TimeToCollide();
                kioskSolid.StopColliding();
            }
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
            _inDialogue = true;
            
            thePlayer.ForceMouseUnlock();

            switch (deliveryCount)
            {
                case 0:
                    break;
                case 1:
                    dialogue.StartConversation(DialogueEnum.Second);
                    break;
                case 2:
                    dialogue.StartConversation(DialogueEnum.Third);
                    break;
                case 3:
                    dialogue.StartConversation(DialogueEnum.Last);
                    break;
                default:
                    break;
            }
            
            //TODO: cutscene with player talking to npc

            
        }

        public void ConversationIsDone()
        {

            _inDialogue = false;
            thePlayer.ChangeBag(BagEnum.NoBag);

            if (deliveryCount == 3)
            {
                
                //bye bye npc
                npc.ByeByeNPC();
                m_AudioSource.Stop();
                m_AudioSource.clip = free;
                m_AudioSource.loop = true;
                m_AudioSource.Play();
            }
        }

        public void Update()
        {
            if (!_paused)
            {
                switch (whereIs)
                {
                    case PlayerLocationState.IsFree:
                        if (theEagleHasLandedOnTheRoof)
                        {
                            if (!done && thePlayer.transform.position.y < -50f)
                            {
                                FindObjectOfType<FaderScript>().FadeAndQuit(FaderScript.FadeType.BEIGE);
                                done = true;
                            }
                        }
                        else
                        {
                            if (thePlayer.transform.position.y > 5f)
                            {
                                kioskRoof.TimeToCollide();
                                theEagleHasLandedOnTheRoof = true;
                            }
                        }

                        break;
                }
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

    public enum BagEnum
    {
        Bag1,
        Bag2,
        Bag3,
        NoBag
    }
}