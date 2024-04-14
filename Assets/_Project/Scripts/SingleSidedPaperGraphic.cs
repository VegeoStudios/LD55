using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSidedPaperGraphic : MonoBehaviour
{
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (Camera.main != null)
        {
            if (Vector3.Dot(Camera.main.transform.forward, transform.forward) > 0)
            {
                canvas.sortingOrder = 1;
            }
            else
            {
                canvas.sortingOrder = -1;
            }
        }
    }
}
