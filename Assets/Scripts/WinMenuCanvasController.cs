using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenuCanvasController : MonoBehaviour
{
    private Canvas _canvasComponent;

    private void Start()
    {
        _canvasComponent = GetComponent<Canvas>();
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

    private void Hide()
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
