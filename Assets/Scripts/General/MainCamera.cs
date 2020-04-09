using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float smoothTime = 0.3f;
    [SerializeField] float maxSpeed = 9.5f;

    private float xVelocity = 0f;
    private float yVelocity = 0f;

    private float _shakeDuration;
    private float _shakeIntensity;
    private Vector3 _originalPos;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        MoveCamera();
        if (_shakeDuration > 0)
        {
            ShakeCamera(_shakeDuration, _shakeIntensity);
        }
    }

    private void MoveCamera()
    {
        float newXpos = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref xVelocity, smoothTime, maxSpeed);
        float newYpos = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref yVelocity, smoothTime, maxSpeed);
        transform.position = new Vector3(newXpos, newYpos, -11f);
    }

    public void ShakeCamera(float shakeDuration, float shakeIntensity)
    {
        _shakeDuration = shakeDuration;
        _shakeIntensity = shakeIntensity;
        _originalPos = transform.position;
        if (_shakeDuration > 0)
        {
            transform.position = _originalPos + Random.insideUnitSphere * shakeIntensity;

            _shakeDuration -= Time.deltaTime * 1f;
        }

    }
}
