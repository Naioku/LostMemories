using System.Collections;
using UnityEngine;

namespace Locomotion.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 8;
        [SerializeField] private float sprintSpeed = 20;
        [SerializeField] private float climbingSpeed = 5;
        [SerializeField] private float lowJumpValue = 20;
        [SerializeField] private float highJumpValue = 30;
        [SerializeField] private float jumpBarLoadingSpeed = 2;
        [SerializeField] private Collider2D legsCollider;

        private Rigidbody2D _rigidbody2D;
        public float _jumpBar;
        private Coroutine _jumpBarCoroutine;

        private bool IsJumpBarLoaded => Mathf.Approximately(_jumpBar, 1);
        private bool IsJumpBarUnloaded => Mathf.Approximately(_jumpBar, 0);

        public bool IsGrounded => legsCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

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

        public void JumpLow() => Jump(lowJumpValue);

        public bool JumpHigh(bool isButtonDown)
        {
            StopJumpBar();

            if (isButtonDown)
            {
                LoadJumpBar();
            }
            else
            {
                if (IsJumpBarLoaded)
                {
                    Jump(highJumpValue);
                    _jumpBar = 0;
                    return true;
                }
                else
                {
                    UnloadJumpBar();
                }
            }

            return false;
        }

        private void LoadJumpBar()
        {
            StopJumpBar();

            _jumpBarCoroutine = StartCoroutine(JumpLoadingCoroutine());
        }
        
        private void UnloadJumpBar()
        {
            StopJumpBar();

            _jumpBarCoroutine = StartCoroutine(JumpUnloadingCoroutine());
        }
        
        private void StopJumpBar()
        {
            if (_jumpBarCoroutine != null)
            {
                StopCoroutine(_jumpBarCoroutine);
            }
        }

        private IEnumerator JumpLoadingCoroutine()
        {
            while (!IsJumpBarLoaded)
            {
                _jumpBar = Mathf.Min(1, _jumpBar + Time.deltaTime * jumpBarLoadingSpeed);
                yield return null;
            }
        }
        
        private IEnumerator JumpUnloadingCoroutine()
        {
            while (!IsJumpBarUnloaded)
            {
                _jumpBar = Mathf.Max(0, _jumpBar - Time.deltaTime * jumpBarLoadingSpeed);
                yield return null;
            }
        }
        
        private void Jump(float jumpValue)
        {
            _rigidbody2D.velocity += new Vector2(0, jumpValue);
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
