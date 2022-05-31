using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenCanvasController : MonoBehaviour
{
    [SerializeField] private Button _memoryCollectionButton;
    [SerializeField] private TextMeshProUGUI _memoryCollectionTMP;

    private AudioPlayerTitleScreen _audioPlayerTitleScreen;
    private static bool _memoryCollectionUnlocked;

    private void Awake()
    {
        _audioPlayerTitleScreen = FindObjectOfType<AudioPlayerTitleScreen>();
    }

    private void Start()
    {
        if (_memoryCollectionUnlocked)
        {
            _memoryCollectionButton.interactable = true;
            _memoryCollectionTMP.color = new Color(0.1960f, 0.1960f, 0.1960f);
        }
    }

    public static void UnlockMemoryCollection()
    {
        _memoryCollectionUnlocked = true;
    }

    public void OnPlay()
    {
        _audioPlayerTitleScreen.DestroyInstance();
        SceneManager.LoadScene("Level");
    }
    
    public void OnMemoryCollection()
    {
        if (_memoryCollectionUnlocked)
        {
            SceneManager.LoadScene("MemoryCollection");
        }
    }
    
    public void OnCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnQuit()
    {
        if (Application.isEditor)
        {
            Debug.Log("Cannot quit the application (Application is editor).");
        }
        else
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
