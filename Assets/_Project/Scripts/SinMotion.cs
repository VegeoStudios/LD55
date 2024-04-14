using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMotion : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float amplitude = 1f;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = startPosition + new Vector3(0, Mathf.Sin(Time.time * speed) * amplitude, 0);
        
    }
}
