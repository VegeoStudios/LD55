using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpInLerpOut : MonoBehaviour
{
    public float duration = 2f;

    private float time = 0;

    private float targetScale = 0f;

    private void OnEnable()
    {
        time = Time.time;
        targetScale = 1f;
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale * Vector3.one, 5 * Time.deltaTime);

        if (Time.time - time > duration)
        {
            targetScale = 0f;

            if (transform.localScale.x < 0.01f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
