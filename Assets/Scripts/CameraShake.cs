using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _shakeMagnitude;

    private Vector3 _initialPosition;
    private bool _isInPlayingState;

    private void Update()
    {
        if (_isInPlayingState)
        {
            _initialPosition = transform.position;
        }
    }

    public void Play()
    {
        _isInPlayingState = true;
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

        _isInPlayingState = false;
    }
}
