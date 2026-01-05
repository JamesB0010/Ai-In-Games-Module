//Credit starter content third person controller pack from unity asset store

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterController
{
    public class PlayerInputValues : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool aim;
        public bool shoot;

        [Header("movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        public void OnMove(InputAction.CallbackContext context)
        {
            this.move = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (this.cursorInputForLook)
                this.look = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            this.jump = context.action.IsPressed();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            this.sprint = context.action.IsPressed();
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            this.aim = context.action.IsPressed();
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            this.shoot = context.action.IsPressed();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(this.cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}