using UnityEngine;

namespace Locomotion.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 8;
        [SerializeField] private float sprintSpeed = 20;
        [SerializeField] private float climbingSpeed = 5;
        
        private Rigidbody2D _rigidbody2D;
        // private AudioPlayer _audioPlayer;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            // _audioPlayer = GetComponent<AudioPlayer>();
        }

        public void MovePlayerHorizontal(float movementDirection, bool isSprinting)
        {
            float speed = isSprinting ? sprintSpeed : movementSpeed;
            _rigidbody2D.velocity = new Vector2(movementDirection * speed, _rigidbody2D.velocity.y);
        }

        private void MovePlayerVertical(float movementDirection)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, movementDirection * climbingSpeed);
        }

        private void StopMovement()
        {
            _rigidbody2D.velocity = Vector2.zero;
        }

        private void FreezePlayer()
        {
            StopMovement();
            // _audioPlayer.DisablePlayingStepsClips();
            _rigidbody2D.isKinematic = true;
        }

        private void UnfreezePlayer()
        {
            _rigidbody2D.isKinematic = false;
        }
    }
}
