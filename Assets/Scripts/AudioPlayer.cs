using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AudioPlayer : MonoBehaviour
{
    [Header("Steps")]
    [SerializeField] private List<AudioClip> stepsClips;
    [SerializeField] [Range(0f, 1f)] private float stepsVolume;
    [SerializeField] private float timeBetweenStepsRun;
    [SerializeField] private float timeBetweenStepsSprint;
    private bool _playingStepsClipsState;
    private bool _readyToPlayNextStep = true;
    private bool _sprintState;
    private Coroutine _stepsCoroutine;

    [Header("Take Scroll")]
    [SerializeField] private List<AudioClip> takeScrollClips;
    [SerializeField] [Range(0f, 1f)] private float takeScrollVolume;
    
    [Header("Die")]
    [SerializeField] private AudioClip dieClip;
    [SerializeField] [Range(0f, 1f)] private float dieVolume;
    
    [Header("Freeze Attack")]
    [SerializeField] private AudioClip freezeAttackClip;
    [SerializeField] [Range(0f, 1f)] private float freezeAttackVolume;
    
    [Header("Katana Attack")]
    [SerializeField] private AudioClip katanaAttackClip;
    [SerializeField] [Range(0f, 1f)] private float katanaAttackVolume;
    
    [Header("Teleport In")]
    [SerializeField] private AudioClip teleportInClip;
    [SerializeField] [Range(0f, 1f)] private float teleportInVolume;
    
    [Header("Teleport Out")]
    [SerializeField] private AudioClip teleportOutClip;
    [SerializeField] [Range(0f, 1f)] private float teleportOutVolume;
    
    [Header("Unroll Scrolls Collection")]
    [SerializeField] private AudioClip unrollScrollsCollectionClip;
    [SerializeField] [Range(0f, 1f)] private float unrollScrollsCollectionVolume;
    
    [Header("Potions")]
    [SerializeField] private AudioClip healthPotionClip;
    [SerializeField] [Range(0f, 1f)] private float healthPotionVolume;
    [SerializeField] private AudioClip manaPotionClip;
    [SerializeField] [Range(0f, 1f)] private float manaPotionVolume;
    
    [Header("Hurt")]
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] [Range(0f, 1f)] private float hurtVolume;

    public void PlayTakeScrollClip(Vector3 position)
    {
        if (takeScrollClips.Count <= 0) return;
        
        var randomizer = new Random();
        AudioSource.PlayClipAtPoint(
            takeScrollClips[randomizer.Next(0, takeScrollClips.Count - 1)],
            new Vector3(position.x, position.y, Camera.main.transform.position.z), 
            takeScrollVolume);
    }

    public void PlayDieClip(Vector3 position)
    {
        if (dieClip == null) return;
        
        AudioSource.PlayClipAtPoint(
            dieClip,
            new Vector3(position.x, position.y, Camera.main.transform.position.z), 
            dieVolume);
    }
    
    public void PlayFreezeAttackClip(Vector3 position)
    {
        if (freezeAttackClip == null) return;
        
        AudioSource.PlayClipAtPoint(
            freezeAttackClip,
            new Vector3(position.x, position.y, Camera.main.transform.position.z), 
            freezeAttackVolume);
    }
    
    public void PlayKatanaAttackClip(Vector3 position)
    {
        if (katanaAttackClip == null) return;
        
        AudioSource.PlayClipAtPoint(
            katanaAttackClip,
            new Vector3(position.x, position.y, Camera.main.transform.position.z), 
            katanaAttackVolume);
    }
    
    public void PlayTeleportInClip(Vector3 position)
    {
        if (teleportInClip == null) return;
        
        AudioSource.PlayClipAtPoint(
            teleportInClip,
            new Vector3(position.x, position.y, Camera.main.transform.position.z), 
            teleportInVolume);
    }
    
    public void PlayTeleportOutClip(Vector3 position)
    {
        if (teleportOutClip == null) return;
        
        AudioSource.PlayClipAtPoint(
            teleportOutClip,
            new Vector3(position.x, position.y, Camera.main.transform.position.z), 
            teleportOutVolume);
    }
    
    public void PlayUnrollScrollsCollectionClip(Vector3 position)
    {
        if (unrollScrollsCollectionClip == null) return;
        
        AudioSource.PlayClipAtPoint(
            unrollScrollsCollectionClip,
            new Vector3(position.x, position.y, Camera.main.transform.position.z), 
            unrollScrollsCollectionVolume);
    }
    
    public void PlayHealthPotionClip(Vector3 position)
    {
        if (healthPotionClip == null) return;
        
        AudioSource.PlayClipAtPoint(
            healthPotionClip,
            new Vector3(position.x, position.y, Camera.main.transform.position.z), 
            healthPotionVolume);
    }
    
    public void PlayManaPotionClip(Vector3 position)
    {
        if (manaPotionClip == null) return;
        
        AudioSource.PlayClipAtPoint(
            manaPotionClip,
            new Vector3(position.x, position.y, Camera.main.transform.position.z), 
            manaPotionVolume);
    }
    
    public void PlayHurtClip(Vector3 position)
    {
        if (hurtClip == null) return;
        
        AudioSource.PlayClipAtPoint(
            hurtClip,
            new Vector3(position.x, position.y, Camera.main.transform.position.z), 
            hurtVolume);
    }
    
    public void EnablePlayingStepsClips()
    {
        if (stepsClips.Count == 0) return;
        _playingStepsClipsState = true;
    }

    public void DisablePlayingStepsClips()
    {
        _playingStepsClipsState = false;
    }
    
    public void EnableSprint()
    {
        _sprintState = true;
        if (_stepsCoroutine != null)
        {
            StopCoroutine(_stepsCoroutine);
        }
        _readyToPlayNextStep = true;
    }

    public void DisableSprint()
    {
        _sprintState = false;
        if (_stepsCoroutine != null)
        {
            StopCoroutine(_stepsCoroutine);
        }
        _readyToPlayNextStep = true;
    }

    private void Update()
    {
        if (_playingStepsClipsState && _readyToPlayNextStep)
        {
            _stepsCoroutine = StartCoroutine(PlayStep());
        }
    }

    private IEnumerator PlayStep()
    {
        _readyToPlayNextStep = false;
        var randomizer = new Random();
        var playerPosition = FindObjectOfType<PlayerController>().transform.position;
        
        AudioSource.PlayClipAtPoint(
            stepsClips[randomizer.Next(0, stepsClips.Count - 1)], 
            new Vector3(playerPosition.x, playerPosition.y, Camera.main.transform.position.z),
            stepsVolume);
        
        yield return _sprintState ? new WaitForSecondsRealtime(timeBetweenStepsSprint) : new WaitForSecondsRealtime(timeBetweenStepsRun);
        _readyToPlayNextStep = true;
    }
}
