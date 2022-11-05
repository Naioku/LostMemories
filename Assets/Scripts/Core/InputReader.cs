using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        public event Action LowJumpEvent;
        public event Action<bool> HighJumpEvent;
        
        public float MovementValue { get; private set; }
        public float ClimbingValue { get; private set; }
        public bool IsSprinting { get; private set; }
        
        private Controls _controls;

        private void Start()
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);

            _controls.Player.Enable();
        }

        private void OnDestroy()
        {
            _controls.Player.Disable();
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<float>();
        }

        public void OnClimb(InputAction.CallbackContext context)
        {
            ClimbingValue = context.ReadValue<float>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            IsSprinting = context.ReadValueAsButton();
        }

        public void OnLowJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            LowJumpEvent?.Invoke();
        }

        public void OnHighJump(InputAction.CallbackContext context)
        {
            if (context.started) HighJumpEvent?.Invoke(true);
            if (context.canceled) HighJumpEvent?.Invoke(false);
        }
    }
}
