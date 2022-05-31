using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _shakeMagnitude;

    private Vector3 _initialPosition;

    public void Play()
    {
        _initialPosition = transform.position;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        var elapsedTime = 0f;
        while (elapsedTime < _shakeDuration)
        {
            transform.position = _initialPosition + (Vector3) Random.insideUnitCircle * _shakeMagnitude;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
