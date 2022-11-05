using UnityEngine;

namespace StateMachine.Player
{
    public abstract class PlayerBaseState : State
    {
        protected readonly PlayerStateMachine StateMachine;

        protected PlayerBaseState(PlayerStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        
        protected void ManageFlippingCharacter()
        {
            if (StateMachine.InputReader.MovementValue != 0)
            {
                FaceToMovementDirection();
            }
        }

        protected void HandleLowJump()
        {
            StateMachine.PlayerMover.JumpLow();
            StateMachine.SwitchState(new PlayerJumpingState(StateMachine));
        }

        protected void HandleHighJump(bool isButtonDown)
        {
            if (!StateMachine.PlayerMover.JumpHigh(isButtonDown)) return;
            StateMachine.SwitchState(new PlayerJumpingState(StateMachine));
        }

        protected bool HasAnimationFinished(string tag)
        {
            return GetNormalizedAnimationTime(StateMachine.Animator, tag) >= 1f;
        }

        private void FaceToMovementDirection()
        {
            StateMachine.transform.localScale = new Vector2(Mathf.Sign(StateMachine.InputReader.MovementValue), 1f);
        }
    }
}
