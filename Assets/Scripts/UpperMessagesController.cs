using TMPro;
using UnityEngine;

public class UpperMessagesController : MonoBehaviour
{
    [TextArea(10, 1)]
    [SerializeField] private string message;
    [SerializeField] private string textMeshProTag;
    [SerializeField] private string canvasTag;

    private TextMeshProUGUI _placeToDisplayMessage;
    private UpperMessagesCanvasController _upperMessagesCanvasController;

    private void Start()
    {
        _placeToDisplayMessage = GameObject.FindWithTag(textMeshProTag).GetComponent<TextMeshProUGUI>();
        _upperMessagesCanvasController = GameObject.FindWithTag(canvasTag).GetComponent<UpperMessagesCanvasController>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Equals("Player")) return;
        _placeToDisplayMessage.text = message;
        _placeToDisplayMessage.autoSizeTextContainer = true;
        _upperMessagesCanvasController.ActivateCanvas();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.tag.Equals("Player")) return;
        _placeToDisplayMessage.text = "";
        _upperMessagesCanvasController.DeactivateCanvas();
    }
}
