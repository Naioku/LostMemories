using UnityEngine;

public class AudioPlayerTitleScreen : MonoBehaviour
{
    private static AudioPlayerTitleScreen _instance;

    private void Awake()
    {
        ManageSingleton();
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
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
}
