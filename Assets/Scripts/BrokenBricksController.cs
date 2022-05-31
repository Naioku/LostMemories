using UnityEngine;

public class BrokenBricksController : MonoBehaviour
{
    private Animator _animator;
    
    private static readonly int IsBreaking = Animator.StringToHash("isBreaking");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            _animator.SetTrigger(IsBreaking);
        }
    }
}
