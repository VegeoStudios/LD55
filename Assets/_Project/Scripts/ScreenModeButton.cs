using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenModeButton : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;

    private static bool isFullscreen = false;

    private void Start()
    {
        isFullscreen = Screen.fullScreen;
        text.text = isFullscreen ? "Go Windowed" : "Go Fullscreen";
    }

    public void Toggle()
    {
        isFullscreen = !isFullscreen;
        if (isFullscreen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }    
        text.text = isFullscreen ? "Go Windowed" : "Go Fullscreen";
    }
}
