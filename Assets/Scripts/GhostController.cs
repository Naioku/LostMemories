using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour, IEnemy
{
    [SerializeField] private float attackSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxChasingRange;
    [SerializeField] private float secondsOfWaitBeforeRemovingFromScene;
    [SerializeField] private float attackDamage;
    [SerializeField] private bool enabledPathfinding;
    
    [Header("Binding with scrolls")]
    [SerializeField] private bool bindWithScrolls;
    [SerializeField] private List<GameObject> scrollsToBind;

    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;
    private Animator _animator;
    private float _originalAnimatorSpeed;
    private GameObject _target;
    private Vector2 _basicPosition;
    private bool _isDead;
    private bool _isFrozen;
    private Pathfinder _pathfinder;
    private AudioPlayer _audioPlayer;

    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int IsDying = Animator.StringToHash("isDying");
    private const float MinHorizontalSpeed = 1f;
    private const float PositionRadius = 0.25f;

    private void Awake()
    {
        _pathfinder = GetComponent<Pathfinder>();
        _audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _originalAnimatorSpeed = _animator.speed;
        _basicPosition = transform.position;
    }

    void Update()
    {
        if (!_isDead && !_isFrozen)
        {
            ManageSpriteFlipping();
        
            if (_target != null)
            {
                if (enabledPathfinding)
                {
                    _pathfinder.DisablePathfinder();
                }
                
                ManageAttackTarget();

                if (IsMaxChasingRangeReached())
                {
                    UnsetTarget();
                }
            }
            else if (enabledPathfinding)
            {
                _pathfinder.EnablePathfinder();
            }
            else
            {
                ManageReturnToBasicPosition();
            }
        }
        else
        {
            StopMovement();
        }
    }

    public void SetTarget(GameObject otherGameObject)
    {
        _basicPosition = transform.position;
        _target = otherGameObject;
        _animator.SetBool(IsAttacking, true);
    }

    public float GetAttackDamage()
    {
        return _isFrozen ? 0f : attackDamage;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public float GetPositionRadius()
    {
        return PositionRadius;
    }

    private void ManageSpriteFlipping()
    {
        if (HasPlayerHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x), 1f);
        }
    }

    private bool HasPlayerHorizontalSpeed()
    {
        return Mathf.Abs(_rigidbody2D.velocity.x) >= MinHorizontalSpeed;
    }

    private void UnsetTarget()
    {
        _target = null;
        _animator.SetBool(IsAttacking, false);
    }

    private void ManageAttackTarget()
    {
        MoveTowardsPosition(_target.transform.position, attackSpeed);
    }

    private bool IsMaxChasingRangeReached()
    {
        return Vector2.Distance(_basicPosition, transform.position) >= maxChasingRange;
    }

    private void ManageReturnToBasicPosition()
    {
        if (Vector2.Distance(_basicPosition, transform.position) > PositionRadius)
        {
            MoveTowardsPosition(_basicPosition, movementSpeed);
        }
        else
        {
           StopMovement();
        }
    }

    private void MoveTowardsPosition(Vector3 targetPosition, float speed)
    {
        Utils.MoveTowardsPosition(targetPosition, speed, transform, _rigidbody2D);
    }

    private void StopMovement()
    {
        _rigidbody2D.velocity = new Vector2(0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            UnsetTarget();
        }
        
        if (col.gameObject.tag.Equals("PlayerWeapon"))
        {
            Die();
        }
        
        if (col.tag.Equals("FreezeProjectile"))
        {
            var freezeProjectileController = col.GetComponent<FreezeProjectileController>();
            StartCoroutine(FreezeGameObject(freezeProjectileController.GetFreezeTime()));
        }
    }

    private void Die()
    {
        _audioPlayer.PlayDieClip(transform.position);
        _animator.SetTrigger(IsDying);
        _isDead = true;
        _boxCollider2D.enabled = false;
        if (bindWithScrolls)
        {
            foreach (var gameObj in scrollsToBind)
            {
                Destroy(gameObj);
            }
        }
        StartCoroutine(RemoveObjectFromScene());
    }

    private IEnumerator RemoveObjectFromScene()
    {
        yield return new WaitForSecondsRealtime(secondsOfWaitBeforeRemovingFromScene);
        
        Destroy(gameObject);
    }
    
    private IEnumerator FreezeGameObject(float freezeTime)
    {
        _isFrozen = true;
        _animator.speed = 0f;
        yield return new WaitForSecondsRealtime(freezeTime);

        _isFrozen = false;
        _animator.speed = _originalAnimatorSpeed;
    }
}
