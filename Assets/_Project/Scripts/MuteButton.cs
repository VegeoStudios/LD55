using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public static bool muted = false;

    [SerializeField] private GameObject unmutedGraphic;
    [SerializeField] private GameObject mutedGraphic;

    private void Start()
    {
        AudioListener.volume = muted ? 0 : 1;
        unmutedGraphic.SetActive(!muted);
        mutedGraphic.SetActive(muted);
    }
    

    public void ToggleMuted()
    {
        muted = !muted;

        AudioListener.volume = muted ? 0 : 1;

        unmutedGraphic.SetActive(!muted);
        mutedGraphic.SetActive(muted);
    }
}
