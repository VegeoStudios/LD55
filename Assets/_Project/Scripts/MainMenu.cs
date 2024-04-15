using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject unmutedGraphic;
    [SerializeField] private GameObject mutedGraphic;

    public void Begin()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ToggleMute(bool isMuted)
    {
        AudioListener.volume = isMuted ? 0 : 1;
        unmutedGraphic.SetActive(!isMuted);
        mutedGraphic.SetActive(isMuted);
    }
}
