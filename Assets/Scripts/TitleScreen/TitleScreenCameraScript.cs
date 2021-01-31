using System;
using UnityEngine;

namespace TitleScreen
{
    public class TitleScreenCameraScript : MonoBehaviour
    {
        public float moveSpeed;

        public float moveRange;

        public void Awake()
        {
            
        }

        public void Update()
        {
            Vector3 current = transform.position;
            float currentZ = current.z;
            
            currentZ += (moveSpeed * Time.deltaTime);

            if (currentZ > moveRange)
            {
                currentZ -= (2 * moveRange);
            }


            transform.position = new Vector3(current.x, current.y, currentZ);
        }
    }
}