using Core;
using Locomotion.Player;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerFallingState : PlayerBaseState
    {
        private static readonly int FallStateHash = Animator.StringToHash("Fall");
        
        private readonly PlayerMover _playerMover;
        private readonly InputReader _inputReader;

        public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            _playerMover = StateMachine.PlayerMover;
            _inputReader = StateMachine.InputReader;
        }

        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(FallStateHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Update()
        {
            if (StateMachine.PlayerMover.IsGrounded)
            {
                StateMachine.SwitchState(new PlayerGroundingState(StateMachine));
                return;
            }
            
            ManageFlippingCharacter();
        }
        
        public override void FixedUpdate()
        {
            _playerMover.MovePlayerHorizontal(_inputReader.MovementValue, _inputReader.IsSprinting);
        }
    }
}
