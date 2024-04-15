using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    // Generated main stats
    public string characterName;
    public Color color;
    public int maxHealth;
    public int maxMana;

    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;

    public Alignment alignment;

    public int paperIndex;

    // Runtime
    public int health;
    public int mana;
    public int gold;
    public int damage;
    public int armor;
    public int lastDayVisited;

    public QuestStatus questStatus;
    public bool alive = true;
    public bool goingHome = false;

    // Chosen
    public Class characterClass;

    private static string[] firstNames;
    private static string[] lastNames;

    public Character()
    {
        color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        GenerateCharacterName();
        GenerateStats();
        damage = 2;
    }

    public void SetClass(Class characterClass)
    {
        this.characterClass = characterClass;
    }

    public void SimulateDay()
    {
        if (health <= 0.5 * maxHealth && Random.Range(0, 100) < 50 && (alignment == Alignment.ChaoticEvil || alignment == Alignment.ChaoticGood || alignment == Alignment.ChaoticNeutral))
        {
            Rest();
        }
        else
        {
            switch (Random.Range(0, 6))
            {
                case 0:
                    SideQuest();
                    break;
                case 1:
                    Rest();
                    break;
                case 2:
                    SkillCheck();
                    break;
                case 3:
                    Shop();
                    break;
                case 4:
                    Encounter();
                    break;
                case 5:
                    if (questStatus == QuestStatus.NotStarted)
                    {
                        QuestObjective();
                    }
                    else
                    {
                        Encounter();
                    }
                    break;
            }
        }

        switch (alignment)
        {            
            case Alignment.ChaoticNeutral:
            case Alignment.LawfulEvil:
            case Alignment.NeutralEvil:
            case Alignment.ChaoticEvil:
                if (Random.Range(0, 100) < 20)
                {
                    ReturnHome();
                }
                break;
            case Alignment.LawfulGood:
                break;
            default:
                if (Random.Range(0, 100) < 7)
                {
                    ReturnHome();
                }
                break;
        }

        if (health <= 0.333 * maxHealth && Random.Range(0, 100) < 60)
        {
            ReturnHome();
        }
    }

    private void ReturnHome()
    {
        goingHome = true;
    }

    private void SideQuest()
    {
        Debug.Log(characterName + " is going on a side quest.");

        gold += Random.Range(2, 15);
        if (Random.Range(0, 100) < 10) DoDamage(Random.Range(1, 5));
        int diff = 0;
        switch (alignment)
        {
            case Alignment.LawfulGood:
                diff = Random.Range(-7, -1);
                break;
            case Alignment.NeutralGood:
            case Alignment.LawfulNeutral:
                diff = Random.Range(-5, -1);
                break;
            case Alignment.ChaoticNeutral:
                diff = Random.Range(-5, 5);
                break;
            case Alignment.LawfulEvil:
            case Alignment.NeutralEvil:
                diff = Random.Range(1, 10);
                break;
            case Alignment.ChaoticEvil:
                diff = Random.Range(5, 20);
                break;
        }

        GameManager.instance.enemyPower += diff;

        if (diff < 0)
        {
            Report.instance.PriorityLog(characterName + " fought off some demons and decreased enemy power by " + Mathf.Abs(diff) + ".");
        }
        else if (diff > 0)
        {
            Report.instance.PriorityLog(characterName + " went on a side quest and helped the demons, increasing enemy power by " + diff + ".");
        }
    }
    
    private void Rest()
    {
        Debug.Log(characterName + " is resting.");

        health += Mathf.RoundToInt(maxHealth * 0.1f);
        health = Mathf.Min(health, maxHealth);
    }

    private void SkillCheck()
    {
        Debug.Log(characterName + " is doing a skill check.");

        bool success = false;
        switch (Random.Range(0, 5))
        {
            case 0:
                success = Random.Range(0, 100) < strength * 5;
                break;
            case 1:
                success = Random.Range(0, 100) < dexterity * 5;
                break;
            case 2:
                success = Random.Range(0, 100) < intelligence * 5;
                break;
            case 3:
                success = Random.Range(0, 100) < wisdom * 5;
                break;
            case 4:
                success = Random.Range(0, 100) < charisma * 5;
                break;
        }

        if (success)
        {
            if (Random.Range(0, 100) < 40)
            {
                int earnings = Random.Range(1, 5);
                gold += earnings;
                Report.instance.Log(characterName + " succeeded a skill check and found " + earnings + " gold pieces.");
            }
        }
        else
        {
            if (Random.Range(0, 100) < 40)
            {
                int damage = Random.Range(1, 2);
                health -= damage;
                Report.instance.Log(characterName + " failed a skill check and lost " + damage + " health.");
            }
        }
    }

    private void Shop()
    {
        Debug.Log(characterName + " is shopping.");

        int price = Random.Range(15, 100);
        if (gold > price)
        {
            if (Random.Range(0, 100) < 50)
            {
                gold -= price;
                armor++;
                Report.instance.Log(characterName + " bought armor for " + price + " gold pieces.");
            }
            else
            {
                gold -= price;
                damage++;
                Report.instance.Log(characterName + " bought a weapon for " + price + " gold pieces.");
            }
        }

    }

    private bool Encounter(bool quest = false)
    {
        Debug.Log(characterName + " is encountering something.");

        int check = 0;

        switch (characterClass)
        {
            case Class.Fighter:
                check = strength;
                break;
            case Class.Rogue:
                check = dexterity;
                break;
            case Class.Wizard:
                check = intelligence;
                break;
            case Class.Cleric:
                check = wisdom;
                break;
            case Class.Sorcerer:
                check = charisma;
                break;
        }
        int roll = Random.Range(0, 100);
        
        if (roll < check * 5 + damage)
        {
            if (Random.Range(0, 100) < 50)
            {
                DoDamage(Random.Range(1, 5));
                Report.instance.Log(characterName + " encountered a monster and took some damage.");
            }

            if (Random.Range(0, 100) < 15)
            {
                LevelUp();
            }

            gold += Random.Range(5, 15);

            return true;
        }
        else
        {
            DoDamage(Random.Range(5, 15));
            return false;
        }

        
    }

    public void LevelUp()
    {
        switch (Random.Range(0, 6))
        {
            case 0:
                strength++;
                break;
            case 1:
                dexterity++;
                break;
            case 2:
                ImproveConstitution();
                break;
            case 3:
                intelligence++;
                break;
            case 4:
                wisdom++;
                break;
            case 5:
                charisma++;
                break;
        }

        Report.instance.PriorityLog(characterName + " leveled up.");
    }

    private void QuestObjective()
    {
        Debug.Log(characterName + " is doing a quest objective.");
        bool success = false;

        if (Random.Range(0, 100) < 70)
        {
            success = Encounter();
        }


        if (success)
        {
            int diff = Random.Range(-20, -1);
            GameManager.instance.enemyPower += diff;
            questStatus = QuestStatus.Completed;
            Report.instance.PriorityLog(characterName + " completed the quest objective and decreased enemy power by " + Mathf.Abs(diff) + ".");
        }
        else
        {
            int diff = Random.Range(5, 1);
            GameManager.instance.enemyPower += diff;
            questStatus = QuestStatus.Failed;
            Report.instance.PriorityLog(characterName + " failed the quest objective and increased enemy power by " + diff + ".");
        }
    }

    private void DoDamage(int damage)
    {
        health -= Mathf.Max(Mathf.RoundToInt(Mathf.Max(damage - armor * 0.2f, 0)), 1);
        if (health <= 0)
        {
            alive = false;
            Report.instance.PriorityLog(characterName + " died.");
        }
    }

    private void ImproveConstitution()
    {
        constitution++;
        int amount = Random.Range(1, 6);
        maxHealth += amount;
        health += amount;
    }

    private void GenerateCharacterName()
    {
        if (firstNames == null)
        {
            TextAsset firstAsset = Resources.Load("first-names") as TextAsset;
            TextAsset lastAsset = Resources.Load("last-names") as TextAsset;

            firstNames = firstAsset.text.Split('\n');
            lastNames = lastAsset.text.Split('\n');
        }

        characterName = firstNames[Random.Range(0, firstNames.Length)].Trim() + " " + lastNames[Random.Range(0, lastNames.Length)].Trim();

    }

    private void GenerateStats()
    {
        maxMana = Mathf.RoundToInt(Mathf.Clamp(Util.NormalDistribution(50, 10), 0, Mathf.Infinity));
        mana = maxMana;
        armor = Mathf.RoundToInt(Mathf.Clamp(Util.NormalDistribution(5, 3), 2, 8));

        strength = Mathf.RoundToInt(Mathf.Clamp(Util.NormalDistribution(10, 3), 0, 20));
        dexterity = Mathf.RoundToInt(Mathf.Clamp(Util.NormalDistribution(10, 3), 0, 20));
        intelligence = Mathf.RoundToInt(Mathf.Clamp(Util.NormalDistribution(10, 3), 0, 20));
        wisdom = Mathf.RoundToInt(Mathf.Clamp(Util.NormalDistribution(10, 3), 0, 20));
        charisma = Mathf.RoundToInt(Mathf.Clamp(Util.NormalDistribution(10, 3), 0, 20));

        maxHealth = 0;
        health = 0;
        int count = Mathf.RoundToInt(Mathf.Clamp(Util.NormalDistribution(10, 2), 0, 20));
        for (int i = 0; i < count; i++)
        {
            ImproveConstitution();
        }

        alignment = (Alignment)Random.Range(0, 9);
    }

    public enum Class
    {
        None,
        Fighter, // Strength
        Rogue, // Dexterity
        Wizard, // Intelligence
        Cleric, // Wisdom
        Sorcerer // Charisma
    }

    public enum Alignment
    {
        LawfulGood,
        NeutralGood,
        ChaoticGood,
        LawfulNeutral,
        TrueNeutral,
        ChaoticNeutral,
        LawfulEvil,
        NeutralEvil,
        ChaoticEvil
    }

    public enum QuestStatus
    {
        NotStarted,
        Completed,
        Failed
    }
}
