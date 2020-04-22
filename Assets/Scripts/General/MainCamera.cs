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
    public bool cellPhoneOpen;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (!cellPhoneOpen)
        {
            MoveCamera();
        }
        if (_shakeDuration > 0)
        {
            ShakeCamera(_shakeDuration, _shakeIntensity);
        }
    }

    public void MoveCamera()
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

    public IEnumerator CellPhoneOpen()
    {
        cellPhoneOpen = true;
        float newPos = transform.position.x + 10f;
        float timer = 1f;
        while (timer > 0)
        {
            float newXpos = Mathf.SmoothDamp(transform.position.x, newPos, ref xVelocity, smoothTime, maxSpeed / 2f);
            transform.position = new Vector3(newXpos, transform.position.y, transform.position.z);
            timer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
    public void CellPhoneClose()
    {
        cellPhoneOpen = false;
    }
}
