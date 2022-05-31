using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsController : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float health = 1f;
    private Slider _healthBar;

    [SerializeField] private float loadingHighJumpSpeed;
    private Slider _jumpBar;
    private float _jumpStrength;

    [Range(0f, 1f)]
    [SerializeField] private float mana = 1f;
    [SerializeField] private float sprintManaCost;
    private Slider _manaBar;

    private const int MaxStatsDecimalPlaces = 2;

    private CameraShake _cameraShake;
    private PlayerController _playerController;
    private bool _loadingHighJumpState;
    private bool _sprintState;
    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _cameraShake = FindObjectOfType<CameraShake>();
        _playerController = GetComponent<PlayerController>();
        _audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    private void Start()
    {
        _healthBar = GameObject.FindWithTag("UIHealthSlider").GetComponent<Slider>();
        _jumpBar = GameObject.FindWithTag("UIJumpSlider").GetComponent<Slider>();
        _manaBar = GameObject.FindWithTag("UIManaSlider").GetComponent<Slider>();
        RefreshHealthBar();
        RefreshManaBar();
        RefreshJumpBar();
    }

    private void Update()
    {
        switch (_loadingHighJumpState)
        {
            case true when !ReadyToHighJump():
                _jumpStrength += loadingHighJumpSpeed * Time.deltaTime;
                break;
            case false when _jumpStrength > 0f:
                _jumpStrength -= loadingHighJumpSpeed * Time.deltaTime;
                break;
        }

        if (_sprintState)
        {
            mana -= sprintManaCost * Time.deltaTime;

            if (!HaveEnoughMana(sprintManaCost))
            {
                _playerController.StopSprint();
            }
        }

        RefreshManaBar();
        RefreshJumpBar();
        ManageDeath();
    }
    
    public bool ReadyToHighJump()
    {
        return _jumpStrength >= _jumpBar.maxValue;
    }

    public void LoadingHighJumpStateOn()
    {
        _loadingHighJumpState = true;
    }

    public void LoadingHighJumpStateOff()
    {
        _loadingHighJumpState = false;
    }

    public void ResetJumpBar()
    {
        _jumpStrength = 0f;
    }

    public void DealManaCost(float manaCost)
    {
        mana = Utils.Round(mana - manaCost, MaxStatsDecimalPlaces);
    }

    public bool HaveEnoughMana(float manaCost)
    {
        return mana - manaCost >= 0f;
    }

    public bool IsSprintPossible()
    {
        return HaveEnoughMana(sprintManaCost);
    }

    public void SprintStateOn()
    {
        _sprintState = true;
    }

    public void SprintStateOff()
    {
        _sprintState = false;
    }

    private void AddHealth(float healthAdded)
    {
        health = Mathf.Clamp(
            Utils.Round(health + healthAdded, MaxStatsDecimalPlaces),
            _healthBar.minValue, 
            _healthBar.maxValue);
    }

    private void AddMana(float manaAdded)
    {
        mana = Mathf.Clamp(
            Utils.Round(mana + manaAdded, MaxStatsDecimalPlaces),
            _manaBar.minValue, 
            _manaBar.maxValue);
    }

    private void RefreshManaBar()
    {
        _manaBar.value = mana;
    }

    private void ManageDeath()
    {
        if (health <= 0f && !_playerController.IsDead())
        {
            _playerController.Die();
        }
    }

    private void RefreshJumpBar()
    {
        _jumpBar.value = _jumpStrength;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "Hazards":
                DealDamage(1f);
                break;
            case "Enemy":
                IEnemy enemyController = col.GetComponent<GhostController>();
                DealDamage(enemyController.GetAttackDamage());
                break;
            case "HealthPotion":
                if (!IsFullOfHealth())
                {
                    _audioPlayer.PlayHealthPotionClip(transform.position);
                    AddHealth(col.GetComponent<PotionsController>().GetChargeValue());
                    Destroy(col.gameObject);
                    RefreshHealthBar();
                }
                break;
            case "ManaPotion":
                if (!IsFullOfMana())
                {
                    _audioPlayer.PlayManaPotionClip(transform.position);
                    AddMana(col.GetComponent<PotionsController>().GetChargeValue());
                    Destroy(col.gameObject);
                }
                break;
        }
    }

    private void DealDamage(float damageAmount)
    {
        _audioPlayer.PlayHurtClip(transform.position);
        health = Utils.Round(health - damageAmount, MaxStatsDecimalPlaces);
        RefreshHealthBar();
        if (!damageAmount.Equals(0f))
        {
            _cameraShake.Play();
        }
    }

    private bool IsFullOfHealth()
    {
        return health >= _healthBar.maxValue;
    }

    private bool IsFullOfMana()
    {
        return mana >= _manaBar.maxValue;
    }

    private void RefreshHealthBar()
    {
        _healthBar.value = health;
    }
}
