using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLerper : MonoBehaviour
{
    public Transform target;
    private Transform previousTarget;
    public float smoothing = 5.0f;

    public bool sound = false;
    private RandomizedSFX sfx;


    private void Start()
    {
        sfx = GetComponent<RandomizedSFX>();
    }

    private void Update()
    {
        if (target != null)
        {
            if (sound)
            {
                if (target != previousTarget)
                {
                    sfx.PlayRandomClip();
                }
            }
            transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, smoothing * Time.deltaTime);
        }
        previousTarget = target;
    }

    public void ClearTarget()
    {
        target = null;
    }

    public void Teleport()
    {
        if (target != null)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
}
