using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Report : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI reportText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMPro.TextMeshProUGUI progressText;
    [SerializeField] private Transform danger;

    public static Report instance;

    private string report = "";
    private string priorityReport = "";

    public ObjectLerper objectLerper;

    private void Start()
    {
        instance = this;
        objectLerper = GetComponent<ObjectLerper>();
    }

    public void PriorityLog(string message)
    {
        priorityReport += "[!] - " + message + "\n";

    }

    public void Log(string message)
    {
        report += "- " + message + "\n";
    }

    public void Clear()
    {
        reportText.text = "";
        priorityReport = "";
        report = "";
    }

    public void Show()
    {
        reportText.text = priorityReport + "\n" + report;

        progressSlider.value = Mathf.Clamp01(GameManager.instance.enemyPower / 200f);
        progressText.text = GameManager.instance.enemyPower + " / 200";

        if (GameManager.instance.enemyPower > 150)
        {
            danger.gameObject.SetActive(true);
        }
        else
        {
            danger.gameObject.SetActive(false);
        }
    }
}
