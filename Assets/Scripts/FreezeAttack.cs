using System;
using UnityEngine;

public class FreezeAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileLifetime;
    [SerializeField] private float manaCost;

    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    public void Attack()
    {
        GameObject instance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var rb = instance.GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.right * transform.localScale.x * projectileSpeed;
        
        _audioPlayer.PlayFreezeAttackClip(transform.position);
        
        Destroy(instance, projectileLifetime);
    }

    public float GetManaCost()
    {
        return manaCost;
    }
}
