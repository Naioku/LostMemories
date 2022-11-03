using Core;
using Locomotion.Player;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerLocomotionState : PlayerBaseState
    {
        private static readonly int IdleStateHash = Animator.StringToHash("Idle");
        private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
        private static readonly int IsSprintingHash = Animator.StringToHash("isSprinting");

        private readonly PlayerMover _playerMover;
        private readonly InputReader _inputReader;
        private readonly Animator _animator;

        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            _playerMover = StateMachine.PlayerMover;
            _inputReader = StateMachine.InputReader;
            _animator = StateMachine.Animator;
        }
        
        public override void Enter()
        {
            _animator.CrossFadeInFixedTime(IdleStateHash, StateMachine.AnimationCrossFadeDuration);
        }

        public override void Update()
        {
            UpdateAnimator();
            if (_inputReader.MovementValue != 0)
            {
                FaceToMovementDirection();
            }
        }

        public override void FixedUpdate()
        {
            _playerMover.MovePlayerHorizontal(_inputReader.MovementValue, _inputReader.IsSprinting);
        }

        private void FaceToMovementDirection()
        {
            StateMachine.transform.localScale = new Vector2(Mathf.Sign(_inputReader.MovementValue), 1f);
        }

        private void UpdateAnimator()
        {
            _animator.SetBool(IsMovingHash, _inputReader.MovementValue != 0);
            _animator.SetBool(IsSprintingHash, _inputReader.IsSprinting);
        }
    }
}
