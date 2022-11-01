using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
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
    }
}
