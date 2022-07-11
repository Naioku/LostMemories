using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] private Transform respawnLocation;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.tag.Equals("Player")) return;
        
        FindObjectOfType<GameSession>().CurrentCheckpoint = respawnLocation;
    }
}
