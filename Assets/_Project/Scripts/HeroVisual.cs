using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HeroVisual : MonoBehaviour
{
    private VisualEffect visualEffect;
    private MeshRenderer meshRenderer;

    [SerializeField] private Texture2D[] summoned;
    [SerializeField] private Texture2D[] success;
    [SerializeField] private Texture2D[] fail;
    [SerializeField] private Texture2D[] injured;

    private void Start()
    {
        visualEffect = GetComponentInChildren<VisualEffect>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetTexture(Situation situation, Color color)
    {
        Texture2D chosenTexture = null;

        switch (situation)
        {
            case Situation.Summoned:
                chosenTexture = summoned[Random.Range(0, summoned.Length)];
                break;
            case Situation.Success:
                chosenTexture = success[Random.Range(0, success.Length)];
                break;
            case Situation.Fail:
                chosenTexture = fail[Random.Range(0, fail.Length)];
                break;
            case Situation.Injured:
                chosenTexture = injured[Random.Range(0, injured.Length)];
                break;
        }

        Debug.Log(chosenTexture.name);
        meshRenderer.material.SetTexture("_BaseMap", chosenTexture);
        meshRenderer.material.SetColor("_BaseColor", color);
    }

    public void GetNextHero()
    {
        GameManager.instance.GetNextHero();
    }

    public void StartVFX()
    {
        visualEffect.Play();
    }

    public void StopVFX()
    {
        visualEffect.Stop();
    }

    public void FinishedTeleport()
    {
        GameManager.instance.FinishedTeleport();
    }

    public enum Situation
    {
        Summoned,
        Success,
        Fail,
        Injured
    }
}
