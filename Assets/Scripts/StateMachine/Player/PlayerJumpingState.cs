using Core;
using Locomotion.Player;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerJumpingState : PlayerBaseState
    {
        private static readonly int JumpStateHash = Animator.StringToHash("Jump");
        
        private readonly PlayerMover _playerMover;
        private readonly InputReader _inputReader;

        public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            _playerMover = StateMachine.PlayerMover;
            _inputReader = StateMachine.InputReader;
        }

        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(JumpStateHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Update()
        {
            if (HasAnimationFinished("Jump"))
            {
                StateMachine.SwitchState(new PlayerFallingState(StateMachine));
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
