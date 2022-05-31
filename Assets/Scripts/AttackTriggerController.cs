using UnityEngine;

public class AttackTriggerController : MonoBehaviour
{
    private GhostController _ghostController;

    private void Awake()
    {
        _ghostController = transform.parent.GetComponent<GhostController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            _ghostController.SetTarget(other.gameObject);
        }
    }
}
