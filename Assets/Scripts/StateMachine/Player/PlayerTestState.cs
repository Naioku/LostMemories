using System.Collections;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerTestState : PlayerBaseState
    {
        private const float WaitTime = 2;

        public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) {}
        
        public override void Enter()
        {
            Debug.Log("Enter");
            StateMachine.StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSecondsRealtime(WaitTime);
            StateMachine.SwitchState(new PlayerTestState(StateMachine));
        }

        public override void Tick()
        {
            Debug.Log("Tick");
        }

        public override void Exit()
        {
            Debug.Log("Exit");
        }
    }
}
