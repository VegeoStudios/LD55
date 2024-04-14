using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HeroDataPaper : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    [SerializeField] private TMPro.TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMPro.TextMeshProUGUI armorText;
    [SerializeField] private TMPro.TextMeshProUGUI goldText;
    [SerializeField] private TMPro.TextMeshProUGUI strText;
    [SerializeField] private TMPro.TextMeshProUGUI dexText;
    [SerializeField] private TMPro.TextMeshProUGUI conText;
    [SerializeField] private TMPro.TextMeshProUGUI intText;
    [SerializeField] private TMPro.TextMeshProUGUI wisText;
    [SerializeField] private TMPro.TextMeshProUGUI chaText;
    [SerializeField] private Transform classStamp;
    [SerializeField] private TMPro.TextMeshProUGUI classText;

    [SerializeField] private Texture2D[] paperSprites;

    public ObjectLerper objectLerper;
    private MeshRenderer meshRenderer;

    private static string[] classNames = new string[] { "NONE", "FIGHTER", "ROGUE", "WIZARD", "CLERIC", "SORCERER" };

    public Character character;

    private void Start()
    {
        objectLerper = GetComponent<ObjectLerper>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void SetCharacter(Character character)
    {

        meshRenderer.material.SetTexture("_BaseMap", paperSprites[character.paperIndex]);

        this.character = character;
        nameText.text = character.characterName;
        armorText.text = character.armor.ToString();
        strText.text = character.strength.ToString();
        dexText.text = character.dexterity.ToString();
        conText.text = character.constitution.ToString();
        intText.text = character.intelligence.ToString();
        wisText.text = character.wisdom.ToString();
        chaText.text = character.charisma.ToString();

        UpdateHealth();
        UpdateGold();
        UpdateClass();

    }

    public void UpdateGold()
    {
        goldText.text = character.gold.ToString();
    }

    public void UpdateHealth()
    {
        healthText.text = character.health.ToString();
        healthSlider.value = (float)character.health / character.maxHealth;
    }

    public void UpdateClass()
    {
        classStamp.gameObject.SetActive(character.characterClass != Character.Class.None);
        if (character.characterClass != Character.Class.None)
        {
            classText.text = classNames[(int)character.characterClass];
        }
    }
}
