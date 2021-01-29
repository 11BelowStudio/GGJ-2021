using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


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
                        //TODO: play the noise of the right key
                    }
                    strafeAxisChange = AxisChangeEnum.Positive;
                    break;
                case AxisChangeEnum.NegativeFromNeutral:
                    if (!canLeft)
                    {
                        //TODO: play the noise of the left key
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
                        //TODO: play the noise of the forward key
                    }
                    forwardAxisChange = AxisChangeEnum.Positive;
                    break;
                case AxisChangeEnum.NegativeFromNeutral:
                    if (!canForward)
                    {
                        //TODO: play the noise of the forward key
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
                    //TODO: play the noise of the spacebar.
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