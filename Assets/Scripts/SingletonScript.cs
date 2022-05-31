using UnityEngine;

public class SingletonScript : MonoBehaviour
{
    private static SingletonScript _instance;

    private void Awake()
    {
        ManageSingleton();
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

    public static SingletonScript GetInstance()
    {
        return _instance;
    }
}
