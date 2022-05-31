using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    [SerializeField] private Vector2 gridSize = new Vector2(1, 1);
    [SerializeField] private Vector2 offSet = new Vector2(0, 0);

    private void OnDrawGizmos()
    {
        Snap();
    }

    private void Snap()
    {
        var position = new Vector3(
            Mathf.Round(transform.position.x / gridSize.x) * gridSize.x,
            Mathf.Round(transform.position.y / gridSize.y) * gridSize.y,
            0f
        );
        
        transform.position = position;
    }
}