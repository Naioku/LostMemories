using System;
using UnityEngine;

public class FreezeProjectileController : MonoBehaviour
{
    [SerializeField] private float freezeTime;

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public float GetFreezeTime()
    {
        return freezeTime;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.tag.Equals("EnemyAttackTrigger"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.tag.Equals("EnemyAttackTrigger"))
        {
            Destroy(gameObject);
        }
    }
}
