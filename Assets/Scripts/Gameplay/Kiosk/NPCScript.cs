using System.Collections;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Kiosk
{
    public class NPCScript : MonoBehaviour
    {
        private bool doWeCareAboutTheNPC;

        private bool npcIsLeaving;

        private Vector3 originLocation;

        private Vector3 npcByeByeLocation;

        private PlayerController pc;


        private float byeByeTime;
        private float byeByeLength = 5F;

        private void Awake()
        {
            originLocation = gameObject.transform.position;

            npcByeByeLocation = new Vector3(originLocation.x, originLocation.y - 10, originLocation.z);
            
            doWeCareAboutTheNPC = true;
            
            pc = FindObjectOfType<PlayerController>();
        }

        public void Update()
        {
            if (doWeCareAboutTheNPC)
            {
                gameObject.transform.LookAt(pc.transform.position, Vector3.up);
                
            }

            if (npcIsLeaving)
            {
                byeByeTime += Time.deltaTime;
                transform.position = Vector3.Lerp(originLocation, npcByeByeLocation, byeByeTime / byeByeLength);
                if (byeByeTime >= byeByeLength)
                {
                    npcIsLeaving = false;
                }
            }
        }

        public void ByeByeNPC()
        {
            byeByeTime = 0F;
            npcIsLeaving = true;
        }


    }
}