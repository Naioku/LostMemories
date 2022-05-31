using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickMenuCanvasController : MonoBehaviour
{
    private Canvas _canvasComponent;

    private void Start()
    {
        _canvasComponent = GetComponent<Canvas>();
    }

    public void OnResume()
    {
        _canvasComponent.enabled = false;
        Hide();
    }

    public void OnDropLife()
    {
        _canvasComponent.enabled = false;
        FindObjectOfType<PlayerController>().Die();
        Hide();
    }

    public void OnPlayAgain()
    {
        Destroy(FindObjectOfType<GameSession>().gameObject);
        SceneManager.LoadScene("Level");
        Hide();
    }

    public void OnTitleScreen()
    {
        Destroy(FindObjectOfType<GameSession>().gameObject);
        SceneManager.LoadScene("TitleScreen");
        Hide();
    }

    public bool IsShown()
    {
        return _canvasComponent.enabled;
    }

    public void Hide()
    {
        _canvasComponent.enabled = false;
        Time.timeScale = 1f;
    }

    public void Show()
    {
        Time.timeScale = 0f;
        _canvasComponent.enabled = true;
    }
}
