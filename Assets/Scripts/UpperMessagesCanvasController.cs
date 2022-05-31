using UnityEngine;

public class UpperMessagesCanvasController : MonoBehaviour
{
    private Canvas _canvasComponent;

    void Start()
    {
        _canvasComponent = GetComponent<Canvas>();
        DeactivateCanvas();
    }

    public void ActivateCanvas()
    {
        _canvasComponent.enabled = true;
    }

    public void DeactivateCanvas()
    {
        _canvasComponent.enabled = false;
    }
}
