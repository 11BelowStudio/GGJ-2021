using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Random = UnityEngine.Random;


namespace Gameplay.Player
{
    public class ControlController: MonoBehaviour
    {

        public bool canForward;
        public bool canBackward;
        public bool canLeft;
        public bool canRight;
        public bool canJump;


        public AxisChangeEnum forwardAxisChange;

        public AxisChangeEnum strafeAxisChange;

        private bool _jumpInput;

        private PlayerController p;

        public bool JumpInput
        {
            get
            {
                bool temp = (canJump && _jumpInput);
                if (temp)
                {
                    _jumpInput = false;
                }
                return temp;
            }
        }

        private Vector2 _movement;
        public Vector2 MovementInput
        {
            get
            {
                return _movement;
            }
        }

        private void Start()
        {
            p = GetComponent<PlayerController>();
            canForward = true;
            canBackward = true;
            canLeft = true;
            canRight = true;
            canJump = false;

            forwardAxisChange = AxisChangeEnum.Neutral;
            strafeAxisChange = AxisChangeEnum.Neutral;

            _jumpInput = false;
            _movement = new Vector2(0, 0);
        }
        
        private void Update()
        {

            float rawStrafeAxis = CrossPlatformInputManager.GetAxisRaw("Horizontal");

            axisEnumChange(ref strafeAxisChange, rawStrafeAxis);

            switch (strafeAxisChange)
            {
                case AxisChangeEnum.PositiveFromNeutral:
                    if (!canRight)
                    {
                        p.gc.MakePickupPlayNoise(ControlPickupEnum.Right);
                    }
                    strafeAxisChange = AxisChangeEnum.Positive;
                    break;
                case AxisChangeEnum.NegativeFromNeutral:
                    if (!canLeft)
                    {
                        p.gc.MakePickupPlayNoise(ControlPickupEnum.Left);
                    }
                    strafeAxisChange = AxisChangeEnum.Negative;
                    break;
            }
                
            float rawForwardAxis = CrossPlatformInputManager.GetAxisRaw("Vertical");
            
            axisEnumChange(ref forwardAxisChange, rawForwardAxis);
            
            switch (forwardAxisChange)
            {
                case AxisChangeEnum.PositiveFromNeutral:
                    if (!canForward)
                    {
                        p.gc.MakePickupPlayNoise(ControlPickupEnum.Forward);
                    }
                    forwardAxisChange = AxisChangeEnum.Positive;
                    break;
                case AxisChangeEnum.NegativeFromNeutral:
                    if (!canBackward)
                    {
                        p.gc.MakePickupPlayNoise(ControlPickupEnum.Backward);
                    }
                    forwardAxisChange = AxisChangeEnum.Negative;
                    break;
            }

            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                if (canJump)
                {
                    _jumpInput = true;
                }
                else
                {
                    p.gc.MakePickupPlayNoise(ControlPickupEnum.Jump);
                }
            }
            
            
            float strafe = CrossPlatformInputManager.GetAxis("Horizontal");
            float advance = CrossPlatformInputManager.GetAxis("Vertical");


            Vector2 desired = new Vector2(strafe, advance);
            if (desired.magnitude > 1)
            {
                desired = desired.normalized;
            }

            float actualStrafe = desired.x;
                
            //if trying to move right
            if (strafe > 0)
            {
                if (!canRight)
                {
                    actualStrafe = 0;
                }
            } else if (!canLeft)
            {
                actualStrafe = 0;
            }

            float actualForward = desired.y;
            if (actualForward > 0)
            {
                if (!canForward)
                {
                    actualForward = 0;
                }
            }
            else if (!canBackward)
            {
                actualForward = 0;
            }

            _movement = new Vector2(actualStrafe, actualForward);


        }
        
        private void axisEnumChange(ref AxisChangeEnum axisEnum, float rawAxis)
        {
            if (Mathf.Approximately(rawAxis, 0f))
            {
                //if raw input is 0, changeEnum is Neutral.
                axisEnum = AxisChangeEnum.Neutral;
            }
            else if (rawAxis < 0f)
            {
                //if rawAxis is negative
                switch (axisEnum)
                {
                    //if the axis wasn't already negative
                    case AxisChangeEnum.PositiveFromNeutral:
                    case AxisChangeEnum.Positive:
                    case AxisChangeEnum.Neutral:
                        //it now is
                        axisEnum = AxisChangeEnum.NegativeFromNeutral;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //if rawAxis is positive
                switch (axisEnum)
                {
                    //if it wasn't already positive
                    case AxisChangeEnum.NegativeFromNeutral:
                    case AxisChangeEnum.Negative:
                    case AxisChangeEnum.Neutral:
                        //it now is.
                        axisEnum = AxisChangeEnum.PositiveFromNeutral;
                        break;
                    default:
                        break;
                }
            }
        }


        public List<ControlPickupEnum> LoseControls(int numberToLose)
        {
            List<ControlPickupEnum> lostControls = new List<ControlPickupEnum>();
            if (canForward)
            {
                lostControls.Add(ControlPickupEnum.Forward);
            }
            if (canBackward)
            {
                lostControls.Add(ControlPickupEnum.Backward);
            }
            if (canLeft)
            {
                lostControls.Add(ControlPickupEnum.Left);
            }
            if (canRight)
            {
                lostControls.Add(ControlPickupEnum.Right);
            }

            if (lostControls.Count <= 1)
            {
                return new List<ControlPickupEnum>();
            }
            else
            {
                bool keepGoing = true;
                int yote = 0;
                do
                {
                    lostControls.RemoveAt(Random.Range(0,lostControls.Count));
                    yote++;
                } while (yote < numberToLose || lostControls.Count > 1);

                return lostControls;
            }
        }
    }

    public enum AxisChangeEnum
    {
        PositiveFromNeutral,
        Positive,
        NegativeFromNeutral,
        Negative,
        Neutral
    }
}