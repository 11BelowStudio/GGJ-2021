using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Player;

namespace Gameplay.ButtonPickup
{
    public class ControlPickup : MonoBehaviour
    {
        public ControlPickupEnum whatPickupIsThis;

        private AudioSource m_AudioSource;

        private GameController gc;

        private PickupParentScript parent;

        private Vector3 speen = new Vector3(0, 30f, 0);
        

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            gc = GameObject.FindObjectOfType<GameController>();
            parent = FindObjectOfType<PickupParentScript>();

        }

        public void PlayTheNoise(AudioClip playThis)
        {
            m_AudioSource.PlayOneShot(playThis);
        }

        public void Update()
        {
            transform.Rotate(speen * Time.deltaTime);
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                PlayerController p = other.GetComponent<PlayerController>();
                switch (whatPickupIsThis)
                {
                    case ControlPickupEnum.Forward:
                        p.controlController.canForward = true;
                        break;
                    case ControlPickupEnum.Backward:
                        p.controlController.canBackward = true;
                        break;
                    case ControlPickupEnum.Left:
                        p.controlController.canLeft = true;
                        break;
                    case ControlPickupEnum.Right:
                        p.controlController.canRight = true;
                        break;
                    case ControlPickupEnum.Jump:
                        p.controlController.canJump = true;
                        break;
                }
                p.PlayNoise(parent.pickedUpNoise);
                parent.PickupPickedUp(whatPickupIsThis);
            }
        }
    }
    
    
    
    
}