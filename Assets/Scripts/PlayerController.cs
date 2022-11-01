using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float climbingSpeed;
    [SerializeField] private float lowJumpSpeed;
    [SerializeField] private float highJumpSpeed;
    [SerializeField] private Transform baseCoordinates;
    [SerializeField] private float timeOfTeleportIn;
    [SerializeField] private float minFallingVelocity;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _legsCollider2D;
    private float _horizontalMovementInput;
    private float _verticalMovementInput;
    private float _originalGravityScale;
    private float _originalAnimatorSpeed;
    private bool _jumpingState;
    private bool _climbingState; // when Player at least touches the climbing trigger
    private bool _isInSprintState;
    private bool _isInTeleportingState;
    private bool _isDead;
    private FreezeAttack _freezeAttack;
    private MemoryViewController _memoryViewController;
    private bool _memoryViewState;
    private PlayerStatsController _playerStatsController;
    private QuickMenuCanvasController _quickMenuCanvasController;
    private HelperCanvasController _helperCanvasController;
    private GameSession _gameSession;
    private AudioPlayer _audioPlayer;

    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsClimbingUp = Animator.StringToHash("isClimbingUp");
    private static readonly int IsClimbingDown = Animator.StringToHash("isClimbingDown");
    private static readonly int IsDying = Animator.StringToHash("isDying");
    private static readonly int IsSprinting = Animator.StringToHash("isSprinting");
    private static readonly int IsTeleporting = Animator.StringToHash("isTeleporting");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");
    private const float MinHorizontalSpeed = 1f;
    private const float MinVerticalSpeed = 1f;

    private void Awake()
    {
        _freezeAttack = GetComponent<FreezeAttack>();
        _memoryViewController = FindObjectOfType<MemoryViewController>();
        _playerStatsController = GetComponent<PlayerStatsController>();
        _gameSession = FindObjectOfType<GameSession>();
        _quickMenuCanvasController = FindObjectOfType<QuickMenuCanvasController>();
        _helperCanvasController = FindObjectOfType<HelperCanvasController>();
        _audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        foreach (var collider2D in GetComponentsInChildren<BoxCollider2D>())
        {
            if (collider2D.gameObject.tag.Equals("PlayerLegs"))
            {
                _legsCollider2D = collider2D;
            }
        }
        _originalGravityScale = _rigidbody2D.gravityScale;
        _originalAnimatorSpeed = _animator.speed;
        
        _gameSession.SetPlayerOnLastCheckpoint();
    }

    private void Update()
    {
        if (_isDead || _isInTeleportingState) return;

        if (!IsInAttackingState() && !_memoryViewState)
        {
            ManageSpriteFlipping();
            ManageMovingAnimation();
            ManageFallingState();
            ManageMovingAudio();
            ManageClimbingAnimation();
            // ManagePlayerMovement();
        }
        else if (!IsInJumpingState())
        {
            // StopMovement();
        }
    }

    public void Die()
    {
        _audioPlayer.PlayDieClip(transform.position);
        _gameSession.DropOneLife();
        _isDead = true;
        _animator.SetTrigger(IsDying);
        // FreezePlayer();
        _playerStatsController.enabled = false;
        Debug.Log("You are dead xd");
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void StopSprint()
    {
        _isInSprintState = false;
        _audioPlayer.DisableSprint();
        _playerStatsController.SprintStateOff();
    }

    private void ManageSpriteFlipping()
    {
        if (HasPlayerHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x), 1f);
        }
    }

    private void ManageMovingAnimation()
    {
        _animator.SetBool(IsMoving, HasPlayerHorizontalSpeed());
        _animator.SetBool(IsSprinting, _isInSprintState);
    }

    private void ManageFallingState()
    {
        if (!IsInClimbingState())
        {
            _animator.SetBool(IsFalling, IsPlayerFalling());
        }
    }

    private bool IsPlayerFalling()
    {
        return _rigidbody2D.velocity.y < minFallingVelocity;
    }

    private void ManageMovingAudio()
    {
        if (HasPlayerHorizontalSpeed())
        {
            _audioPlayer.EnablePlayingStepsClips();
        }
        else
        {
            _audioPlayer.DisablePlayingStepsClips();
        }
    }

    private bool HasPlayerHorizontalSpeed()
    {
        return Mathf.Abs(_rigidbody2D.velocity.x) >= MinHorizontalSpeed;
    }

    private void ManageClimbingAnimation()
    {
        if (IsInClimbingState() &&
            (HasPlayerVerticalSpeed() || !_legsCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))))
        {
            if (HasPlayerVerticalSpeed())
            {
                if (Mathf.Sign(_rigidbody2D.velocity.y) == 1f)
                {
                    _animator.SetBool(IsClimbingDown, false);
                    _animator.SetBool(IsClimbingUp, true);
                }
                else if (Mathf.Sign(_rigidbody2D.velocity.y) == -1f &&
                         !_rigidbody2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
                {
                    _animator.SetBool(IsClimbingUp, false);
                    _animator.SetBool(IsClimbingDown, true);
                }
            }

            _animator.speed = HasPlayerVerticalSpeed() ? _originalAnimatorSpeed : 0f;
        }
        else
        {
            _animator.SetBool(IsClimbingUp, false);
            _animator.SetBool(IsClimbingDown, false);
            _animator.speed = _originalAnimatorSpeed;
        }
    }

    private bool HasPlayerVerticalSpeed()
    {
        return Mathf.Abs(_rigidbody2D.velocity.y) >= MinVerticalSpeed;
    }

    private void ManagePlayerMovement()
    {
        // MovePlayerHorizontal();
        //
        // if (IsClimbingPossible() && !IsInJumpingState())
        // {
        //     MovePlayerVertical();
        // }
    }

    private bool IsClimbingPossible()
    {
        return _rigidbody2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));
    }

    // private void MovePlayerHorizontal()
    // {
    //     float speed = _isInSprintState ? sprintSpeed : movementSpeed;
    //     _rigidbody2D.velocity = new Vector2(_horizontalMovementInput * speed, _rigidbody2D.velocity.y);
    // }
    //
    // private void MovePlayerVertical()
    // {
    //     _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _verticalMovementInput * climbingSpeed);
    // }
    //
    // private void StopMovement()
    // {
    //     _rigidbody2D.velocity = Vector2.zero;
    // }
    //
    // private void FreezePlayer()
    // {
    //     StopMovement();
    //     _audioPlayer.DisablePlayingStepsClips();
    //     _rigidbody2D.isKinematic = true;
    // }
    //
    // private void UnfreezePlayer()
    // {
    //     _rigidbody2D.isKinematic = false;
    // }

    private void OnCloseAttack()
    {
        if (!IsInAttackingState() &&
            (!IsInClimbingState() || _legsCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))))
        {
            _audioPlayer.PlayKatanaAttackClip(transform.position);
            _animator.SetTrigger(IsAttacking);
        }
    }

    private void OnMoveHorizontal(InputValue value)
    {
        _horizontalMovementInput = value.Get<Single>();
    }
    
    private void OnMoveVertical(InputValue value)
    {
        _verticalMovementInput = value.Get<Single>();
    }

    private void OnSprint(InputValue value)
    {
        if (_isDead) return;
        
        Single input = value.Get<Single>();

        if (input.Equals(1) && IsSprintPossible())
        {
            _isInSprintState = true;
            _playerStatsController.SprintStateOn();
            _audioPlayer.EnableSprint();
        }
        else
        {
            _isInSprintState = false;
            _playerStatsController.SprintStateOff();
            _audioPlayer.DisableSprint();
        }
    }

    private bool IsSprintPossible()
    {
        return _playerStatsController.IsSprintPossible() && !IsInClimbingState();
    }

    private void OnLowJump()
    {
        if (_isDead) return;
        
        if (ReadyToLowJump())
        {
            JumpingStateOn();
            ClimbingStateOff();
            _rigidbody2D.velocity += new Vector2(0f, lowJumpSpeed);
        }
    }

    private bool ReadyToLowJump()
    {
        return (_legsCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
               _legsCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"))) &&
               !IsInAttackingState();
    }

    private void OnHighJump(InputValue value)
    {
        if (_isDead) return;
        {
            _playerStatsController.LoadingHighJumpStateOff();
        }

        Single pressingValue = value.Get<Single>();
        switch (pressingValue)
        {
            case 1:
                _playerStatsController.LoadingHighJumpStateOn();
                break;
            case 0:
                _playerStatsController.LoadingHighJumpStateOff();
                if (ReadyToHighJump())
                {
                    _playerStatsController.ResetJumpBar();
                    JumpingStateOn();
                    ClimbingStateOff();
                    _rigidbody2D.velocity += new Vector2(0f, highJumpSpeed);
                }
                break;
        }
    }

    private bool ReadyToHighJump()
    {
        return ReadyToLowJump() && _playerStatsController.ReadyToHighJump();
    }

    private void OnDistanceAttack()
    {
        if (!_playerStatsController.HaveEnoughMana(_freezeAttack.GetManaCost()) ||
            _isDead) return;
        
        _playerStatsController.DealManaCost(_freezeAttack.GetManaCost());
        _freezeAttack.Attack();
    }

    private void OnShowMemoryView(InputValue value)
    {
        Single pressingValue = value.Get<Single>();
        switch (pressingValue)
        {
            case 1:
                _audioPlayer.PlayUnrollScrollsCollectionClip(transform.position);
                _memoryViewState = true;
                _memoryViewController.ShowWholeView();
                break;
            case 0:
                _memoryViewState = false;
                _memoryViewController.HideWholeView();
                break;
        }
    }
    
    private void OnSwitchMemoryViewRight(InputValue value)
    {
        _memoryViewController.SwitchViewToTheRight();
    }
    
    private void OnSwitchMemoryViewLeft(InputValue value)
    {
        _memoryViewController.SwitchViewToTheLeft();
    }

    private void OnQuickMenu()
    {
        if (_quickMenuCanvasController.IsShown())
        {
            _quickMenuCanvasController.Hide();
        }
        else
        {
            _quickMenuCanvasController.Show();
        }
    }
    
    private void OnToggleHelper()
    {
        if (_helperCanvasController.IsShown())
        {
            _helperCanvasController.Hide();
        }
        else
        {
            _helperCanvasController.Show();
        }
    }
    
    private bool IsInAttackingState()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }

    private bool IsInClimbingState()
    {
        return _climbingState;
    }

    private void ClimbingStateOn()
    {
        _rigidbody2D.gravityScale = 0f;
        _climbingState = true;
    }

    private void ClimbingStateOff()
    {
        _rigidbody2D.gravityScale = _originalGravityScale;
        _climbingState = false;
    }

    private bool IsInJumpingState()
    {
        return _jumpingState;
    }

    private void JumpingStateOn()
    {
        _jumpingState = true;
        _rigidbody2D.gravityScale = _originalGravityScale;
        _animator.SetBool(IsJumping, true);
    }

    public void JumpingStateOff()
    {
        _jumpingState = false;
        _animator.SetBool(IsJumping, false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Teleport"))
        {
            StartCoroutine(StartTeleporting(baseCoordinates.position));
        }
    }

    private IEnumerator StartTeleporting(Vector3 coordinates)
    {
        _isInTeleportingState = true;
        
        _audioPlayer.PlayTeleportInClip(transform.position);
        _animator.SetBool(IsTeleporting, true);
        
        // FreezePlayer();
        yield return new WaitForSecondsRealtime(timeOfTeleportIn);
        
        transform.position = coordinates;
        _audioPlayer.PlayTeleportOutClip(transform.position);
        _animator.SetBool(IsTeleporting, false);
        yield return new WaitForSecondsRealtime(timeOfTeleportIn);

        // UnfreezePlayer();
        _isInTeleportingState = false;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag.Equals("Climbing") && (!IsInJumpingState() || HasPlayerVerticalInput()))
        {
            ClimbingStateOn();
            JumpingStateOff();
        }
    }
    
    private bool HasPlayerVerticalInput()
    {
        return Mathf.Abs(_verticalMovementInput) >= MinVerticalSpeed;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Climbing"))
        {
            ClimbingStateOff();
        }
    }
}
