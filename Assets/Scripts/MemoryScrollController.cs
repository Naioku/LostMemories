using UnityEngine;

public class MemoryScrollController : MonoBehaviour
{
    [SerializeField] private GameObject contentContainer;
    
    private MemoryViewController _memoryViewController;
    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _memoryViewController = FindObjectOfType<MemoryViewController>();
        _audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            GetScrollToPlayer();
        }
    }

    public void GetScrollToPlayer()
    {
        _audioPlayer.PlayTakeScrollClip(transform.position);
        contentContainer.SetActive(true);
        _memoryViewController.RefreshScore();
        Destroy(gameObject);
    }
}
