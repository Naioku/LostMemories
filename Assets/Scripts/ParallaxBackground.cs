using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Camera cam;
    // [SerializeField] private Transform subject;

    private Transform _subject;
    private Vector2 _startPosition;
    private float _startZ;
    private Vector2 Travel => (Vector2) cam.transform.position - _startPosition;

    private float DistanceFromSubject => transform.position.z - _subject.position.z;

    private float ClippingPlane =>
        cam.transform.position.z + (DistanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane);

    private float ParallaxFactor => Mathf.Abs(DistanceFromSubject) / ClippingPlane;

    private void Start()
    {
        _subject = FindObjectOfType<PlayerController>().transform;
        _startPosition = transform.position;
        _startZ = transform.position.z;
    }

    private void Update()
    {
        Vector2 newPosition = _startPosition + Travel * ParallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, _startZ);
    }
}
