using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int enemyPower = 50;
    public int budget = 80;
    public int day = 0;

    public List<Character> characterList = new List<Character>();

    public Queue<Character> characterQueue = new Queue<Character>();

    public Character currentCharacter = null;

    [SerializeField] private Animator heroAnimator;
    [SerializeField] private HeroVisual heroVisual;

    [SerializeField] private Animator daySwitch;
    [SerializeField] private HeroDataPaper heroDataPaper;

    [SerializeField] private Transform pilePosition;
    [SerializeField] private Transform summonPosition;
    [SerializeField] private Transform viewPosition;
    [SerializeField] private Transform reportPosition;

    [SerializeField] private Transform heroBuyScreen;
    [SerializeField] private Transform characterActionsScreen;
    [SerializeField] private Transform classSelectionScreen;
    [SerializeField] private Transform reportUI;

    private bool heroBoughtToday = false;

    private void Start()
    {
        instance = this;
    }

    public void StartGame()
    {

        GetNextHero();
    }

    public void SimulateDay()
    {
        Report.instance.Clear();
        foreach (Character character in characterList)
        {
            character.SimulateDay();
        }

        for (int i = 0; i < characterList.Count; i++)
        {
            Character character = characterList[i];
            if (character.health <= 0)
            {
                characterList.RemoveAt(i);
                i--;
            }
            else if (character.goingHome)
            {
                character.goingHome = false;
                characterList.RemoveAt(i);
                i--;
                characterQueue.Enqueue(character);
            }
        }

        day++;
        heroBoughtToday = false;
        budget += 10;
        enemyPower += 2;

        enemyPower = Mathf.Clamp(enemyPower, 0, 200);

        if (enemyPower >= 200)
        {

        }
        else if (enemyPower <= 0)
        {

        }
        else
        {
            Report.instance.Show();
        }
    }

    public void ViewReport()
    {
        Report.instance.objectLerper.target = viewPosition;
        reportUI.gameObject.SetActive(true);
    }

    public void StopReport()
    {
        Report.instance.objectLerper.target = reportPosition;
        GetNextHero();
    }

    public void GetNextHero()
    {
        if (characterQueue.Count > 0)
        {
            currentCharacter = characterQueue.Dequeue();

            heroDataPaper.SetCharacter(currentCharacter);

            if (currentCharacter.characterClass == Character.Class.None)
            {
                heroDataPaper.objectLerper.target = summonPosition;
                heroAnimator.SetTrigger("Teleport");
            }
            else
            {
                heroDataPaper.objectLerper.target = pilePosition;
                heroAnimator.SetTrigger("Enter");
            }

            heroDataPaper.objectLerper.Teleport();

            heroDataPaper.objectLerper.target = viewPosition;

            if (currentCharacter.characterClass == Character.Class.None)
            {
                heroVisual.SetTexture(HeroVisual.Situation.Summoned, currentCharacter.color);
            }
            else if (currentCharacter.health <= 0.5 * currentCharacter.maxHealth)
            {
                heroVisual.SetTexture(HeroVisual.Situation.Injured, currentCharacter.color);
            }
            else if (currentCharacter.questStatus == Character.QuestStatus.Completed)
            {
                heroVisual.SetTexture(HeroVisual.Situation.Success, currentCharacter.color);
            }
            else
            {
                heroVisual.SetTexture(HeroVisual.Situation.Fail, currentCharacter.color);
            }

        }
        else
        {
            if (heroBoughtToday || budget < 50)
            {
                GoToNextDay();
            }
            else
            {
                heroBuyScreen.gameObject.SetActive(true);
            }
        }
    }

    public void FinishedTeleport()
    {
        if (currentCharacter.characterClass == Character.Class.None)
        {
            classSelectionScreen.gameObject.SetActive(true);
            return;
        }
        else
        {
            characterActionsScreen.gameObject.SetActive(true);
        }

        if (currentCharacter.questStatus == Character.QuestStatus.Completed)
        {
            SpeechBubbleController.instance.Say(currentCharacter.alignment, SpeechBubbleController.Situation.Success);
        }
        else
        {
            SpeechBubbleController.instance.Say(currentCharacter.alignment, SpeechBubbleController.Situation.Fail);
        }

        if (currentCharacter.health <= 0.5 * currentCharacter.maxHealth)
        {
            SpeechBubbleController.instance.Say(currentCharacter.alignment, SpeechBubbleController.Situation.Injured);
        }
    }

    public void GoToNextDay()
    {
        daySwitch.SetTrigger("Switch");
    }

    public void SetClass(int i)
    {
        SpeechBubbleController.instance.Say(currentCharacter.alignment, SpeechBubbleController.Situation.Summ2);
        currentCharacter.characterClass = (Character.Class)(i + 1);
        classSelectionScreen.gameObject.SetActive(false);
        characterActionsScreen.gameObject.SetActive(true);
    }

    public void BuyHero()
    {
        heroBuyScreen.gameObject.SetActive(false);
        heroBoughtToday = true;
        budget -= 50;
        Character newCharacter = new Character();
        characterQueue.Enqueue(newCharacter);
        GetNextHero();

        SpeechBubbleController.instance.Say(newCharacter.alignment, SpeechBubbleController.Situation.Summ1);
    }

    public void SendOnQuest()
    {
        SpeechBubbleController.instance.Say(currentCharacter.alignment, SpeechBubbleController.Situation.Quest);
        characterList.Add(currentCharacter);
        heroAnimator.SetTrigger("Leave");
        currentCharacter = null;

        heroDataPaper.objectLerper.target = pilePosition;

        
    }

    public void Detain()
    {
        SpeechBubbleController.instance.Say(currentCharacter.alignment, SpeechBubbleController.Situation.Imprison);

        heroAnimator.SetTrigger("Leave");
        currentCharacter = null;

        heroDataPaper.objectLerper.target = summonPosition;
    }

    public void IncrementGold()
    {
        if (budget > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                currentCharacter.gold += budget;
                budget = 0;
            }
            else
            {
                budget--;
                currentCharacter.gold++;
            }
            heroDataPaper.UpdateGold();
        }
    }

    public void DecrementGold()
    {
        if (currentCharacter.gold > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                budget += currentCharacter.gold;
                currentCharacter.gold = 0;
            }
            else
            {
                budget++;
                currentCharacter.gold--;
            }
            heroDataPaper.UpdateGold();
        }
    }

    public void Heal()
    {
        if (budget >= 20)
        {
            budget -= 20;
            currentCharacter.health = currentCharacter.maxHealth;
            heroDataPaper.UpdateHealth();

            heroVisual.SetTexture(HeroVisual.Situation.Success, currentCharacter.color);
        }
    }

    public void LevelUp()
    {
        if (budget >= 80)
        {
            budget -= 80;
            currentCharacter.LevelUp();
            heroDataPaper.SetCharacter(currentCharacter);
        }
    }

    private enum DayEvent
    {
        None,
        Encounter,
        SideQuest,
        Rest,
        SkillCheck,
        Shop
    }
}
