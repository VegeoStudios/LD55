using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringCandleLight : MonoBehaviour
{
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            //GetComponent<Light>().intensity = Random.Range(0.5f, 1.0f);

            transform.localPosition = originalPosition + new Vector3(Random.Range(-0.01f, 0.01f), 0, Random.Range(-0.01f, 0.01f));
        }
    }
}
