using Core;
using Locomotion.Player;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerGroundingState : PlayerBaseState
    {
        private static readonly int GroundStateHash = Animator.StringToHash("Ground");

        private readonly PlayerMover _playerMover;
        private readonly InputReader _inputReader;

        public PlayerGroundingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            _playerMover = StateMachine.PlayerMover;
            _inputReader = StateMachine.InputReader;
        }

        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(GroundStateHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Update()
        {
            if (HasAnimationFinished("Ground"))
            {
                StateMachine.SwitchState(new PlayerLocomotionState(StateMachine));
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
