using Core;
using Locomotion.Player;

namespace StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {
        internal PlayerMover PlayerMover { get; private set; }
        internal InputReader InputReader { get; private set; }

        private void Awake()
        {
            PlayerMover = GetComponent<PlayerMover>();
            InputReader = GetComponent<InputReader>();
        }

        private void Start()
        {
            SwitchState(new PlayerLocomotion(this));
        }
    }
}
