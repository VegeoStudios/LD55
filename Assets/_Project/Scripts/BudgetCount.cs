using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BudgetCount : MonoBehaviour
{
    private TMPro.TextMeshPro text;

    private void Start()
    {
        text = GetComponent<TMPro.TextMeshPro>();
    }

    private void Update()
    {
        text.text = GameManager.instance.budget.ToString();
    }
}
