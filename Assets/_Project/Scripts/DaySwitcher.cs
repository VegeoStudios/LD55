using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaySwitcher : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI dayText;

    public void ClearNum()
    {
        dayText.text = "";
    }

    public void SetNum()
    {
        GameManager.instance.SimulateDay();
        dayText.text = GameManager.instance.day.ToString();
    }

    public void ViewReport()
    {
        GameManager.instance.ViewReport();
    }
}
