using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesTextMeshPro;
    [SerializeField] private int lives = 3;
    [SerializeField] private float secondsWaitToRestartLevel = 2f;

    private static GameSession _instance;
    private UpperMessagesCanvasController _upperMessagesCanvasController;

    public Transform CurrentCheckpoint { get; set; } = null;

    private void Awake()
    {
        ManageSingleton();
    }

    private void Start()
    {
        RefreshLives();
    }

    public void DropOneLife()
    {
        lives--;
        if (lives < 0)
        {
            StartCoroutine(GameOver());
        }
        else
        {
            RefreshLives();
            StartCoroutine(RestartLevel());
        }
    }

    public void SetPlayerOnLastCheckpoint()
    {
        if (CurrentCheckpoint == null) return;
        
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = CurrentCheckpoint.position;
    }

    private void ManageSingleton()
    {
        if (_instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSecondsRealtime(secondsWaitToRestartLevel);
        SceneManager.LoadScene("Level");
    }

    private void RefreshLives()
    {
        livesTextMeshPro.text = "x" + lives;
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSecondsRealtime(secondsWaitToRestartLevel);
        SceneManager.LoadScene("GameOver");
        Destroy(gameObject);
    }
}
