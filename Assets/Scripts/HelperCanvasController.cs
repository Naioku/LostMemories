using UnityEngine;

public class HelperCanvasController : MonoBehaviour
{
    private Canvas _canvasComponent;

    private void Start()
    {
        _canvasComponent = GetComponent<Canvas>();
    }
    
    public bool IsShown()
    {
        return _canvasComponent.enabled;
    }

    public void Hide()
    {
        _canvasComponent.enabled = false;
        Time.timeScale = 1f;
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerController>().enabled = true;
    }

    public void Show()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerController>().enabled = false;
        Time.timeScale = 0f;
        _canvasComponent.enabled = true;
    }
}
