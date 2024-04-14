using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float minAngle = 10f;
    [SerializeField] private float maxAngle = 50f;
    [SerializeField] private float minPercent = 0.2f;
    [SerializeField] private float maxPercent = 0.8f;
    [SerializeField] private float smoothing;

    private float angle = 10f;

    private void Update()
    {
        float targetAngle = Mathf.Lerp(maxAngle, minAngle, Mathf.Clamp01(Mathf.InverseLerp(minPercent, maxPercent, Input.mousePosition.y / Screen.height)));

        angle = Mathf.Lerp(angle, targetAngle, smoothing * Time.deltaTime);

        transform.localEulerAngles = new Vector3(angle, 0, 0);
    }

}
