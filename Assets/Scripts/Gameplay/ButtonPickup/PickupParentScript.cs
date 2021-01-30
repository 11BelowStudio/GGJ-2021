using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gameplay.ButtonPickup
{
    public class PickupParentScript : MonoBehaviour
    {
        public AudioClip[] pickupNoises;
        
        public ControlPickup leftPickup;

        public ControlPickup rightPickup;

        public ControlPickup forwardPickup;

        public ControlPickup backPickup;

        public ControlPickup jumpPickup;

        public GameObject holdingPosition;
        
        public AudioClip pickedUpNoise;

        private List<GameObject> pickupPositions;
        private void Awake()
        {
            pickupPositions = new List<GameObject>(GameObject.FindGameObjectsWithTag("PickupPosition"));
            jumpPickup.transform.position = GameObject.Find("JumpPickupPosition").transform.position;
            Vector3 holdPos = holdingPosition.transform.position;
            forwardPickup.transform.position = holdPos;
            backPickup.transform.position = holdPos;
            leftPickup.transform.position = holdPos;
            rightPickup.transform.position = holdPos;
        }

        public void HidePickups(List<ControlPickupEnum> hideThese)
        {
            foreach(ControlPickupEnum c in hideThese)
            {
                MovePickup(c, pickupPositions[Random.Range(0, pickupPositions.Count - 1)].transform.position);
            }
        }

        private void MovePickup(ControlPickupEnum moveThis, Vector3 moveItHere)
        {
            switch (moveThis)
            {
                case ControlPickupEnum.Forward:
                    forwardPickup.transform.position = moveItHere;
                    break;
                case ControlPickupEnum.Backward:
                    backPickup.transform.position = moveItHere;
                    break;
                case ControlPickupEnum.Left:
                    leftPickup.transform.position = moveItHere;
                    break;
                case ControlPickupEnum.Right:
                    rightPickup.transform.position = moveItHere;
                    break;
                case ControlPickupEnum.Jump:
                    jumpPickup.transform.position = moveItHere;
                    break;
            }
        }
        
        public void MakePickupPlayNoise(ControlPickupEnum noisemaker)
        {
            int n = Random.Range(1, pickupNoises.Length);
            AudioClip playThis = pickupNoises[n];
            pickupNoises[n] = pickupNoises[0];
            pickupNoises[0] = playThis;
            switch (noisemaker)
            {
                case ControlPickupEnum.Forward:
                    forwardPickup.PlayTheNoise(playThis);
                    break;
                case ControlPickupEnum.Backward:
                    backPickup.PlayTheNoise(playThis);
                    break;
                case ControlPickupEnum.Left:
                    leftPickup.PlayTheNoise(playThis);
                    break;
                case ControlPickupEnum.Right:
                    rightPickup.PlayTheNoise(playThis);
                    break;
                case ControlPickupEnum.Jump:
                    jumpPickup.PlayTheNoise(playThis);
                    break;
            }
        }

        public void PickupPickedUp(ControlPickupEnum thePickup)
        {
            MovePickup(thePickup, holdingPosition.transform.position);
        }
    }
}