using Core;
using Locomotion.Player;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerLocomotion : PlayerBaseState
    {
        private PlayerMover _playerMover;
        private InputReader _inputReader;
        
        public PlayerLocomotion(PlayerStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            _playerMover = StateMachine.PlayerMover;
            _inputReader = StateMachine.InputReader;
        }

        public override void Tick()
        {
            Debug.Log("MovementValue: " + _inputReader.MovementValue);
            _playerMover.MovePlayerHorizontal(_inputReader.MovementValue, _inputReader.IsSprinting);
        }

        public override void Exit()
        {
        }
    }
}
