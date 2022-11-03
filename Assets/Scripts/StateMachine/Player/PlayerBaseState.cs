using Core;
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
        
        protected void FaceMovementDirection(InputReader inputReader)
        {
            StateMachine.transform.localScale = new Vector2(Mathf.Sign(inputReader.MovementValue), 1f);
        }
    }
}
