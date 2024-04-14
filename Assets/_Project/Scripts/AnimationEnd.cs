using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnd : MonoBehaviour
{
    [SerializeField] private Transform introUI;

    public void OnAnimationEnd()
    {
        introUI.gameObject.SetActive(true);
        GetComponent<Animator>().enabled = false;
    }
}
