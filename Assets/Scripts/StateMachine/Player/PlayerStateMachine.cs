using Core;
using Locomotion.Player;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] internal float AnimationCrossFadeDuration { get; private set; } = 0;
        
        internal PlayerMover PlayerMover { get; private set; }
        internal InputReader InputReader { get; private set; }
        internal Animator Animator { get; private set; }

        private void Awake()
        {
            PlayerMover = GetComponent<PlayerMover>();
            InputReader = GetComponent<InputReader>();
            Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SwitchState(new PlayerLocomotionState(this));
        }
    }
}
