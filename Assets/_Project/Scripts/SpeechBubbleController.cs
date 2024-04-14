using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpeechBubbleController;

public class SpeechBubbleController : MonoBehaviour
{
    public static SpeechBubbleController instance;

    private Transform speechBubble1;
    private Transform speechBubble2;

    private TMPro.TextMeshProUGUI speechBubbleText1;
    private TMPro.TextMeshProUGUI speechBubbleText2;

    private RandomizedSFX sfx1;
    private RandomizedSFX sfx2;

    [SerializeField] private Dialogue[] Dialogues;

    private float lastMessageTime = 0;
    private float messageCooldown = 2;

    public enum Situation
    {
        Summ1,
        Summ2,
        Quest,
        Imprison,
        Success,
        Fail,
        Injured
    }

    private void Start()
    {
        instance = this;

        speechBubble1 = transform.GetChild(0);
        speechBubble2 = transform.GetChild(1);

        speechBubbleText1 = speechBubble1.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        speechBubbleText2 = speechBubble2.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        sfx1 = speechBubble1.GetComponent<RandomizedSFX>();
        sfx2 = speechBubble2.GetComponent<RandomizedSFX>();
    }

    public void Say(Character.Alignment alignment, Situation situation)
    {
        if (Time.time - lastMessageTime < messageCooldown)
        {
            StartCoroutine(Wait(messageCooldown - (Time.time - lastMessageTime), alignment, situation));
            return;
        }

        List<string> set = new List<string>();
        Dialogue dialogue = Dialogues[(int)alignment];
        switch (situation)
        {
            case Situation.Summ1:
                set = dialogue.summoned1;
                break;
            case Situation.Summ2:
                set = dialogue.summoned2;
                break;
            case Situation.Quest:
                set = dialogue.responses_cooperate;
                break;
            case Situation.Imprison:
                set = dialogue.responses_imprison;
                break;
            case Situation.Success:
                set = dialogue.succeeded;
                break;
            case Situation.Fail:
                set = dialogue.failed;
                break;
            case Situation.Injured:
                set = dialogue.injured;
                break;
        }

        string message = set[Random.Range(0, set.Count)];

        if (speechBubble1.gameObject.activeSelf)
        {
            speechBubbleText2.text = message;
            speechBubble2.gameObject.SetActive(true);
            sfx2.PlayRandomClip();
        }
        else
        {
            speechBubbleText1.text = message;
            speechBubble1.gameObject.SetActive(true);
            sfx1.PlayRandomClip();
        }

        lastMessageTime = Time.time;
    }

    private IEnumerator Wait(float seconds, Character.Alignment alignment, Situation situation)
    {
        yield return new WaitForSeconds(seconds);

        Say(alignment, situation);
    }

    public void Clear()
    {
        speechBubbleText1.text = "";
        speechBubbleText2.text = "";
        speechBubble1.gameObject.SetActive(false);
        speechBubble2.gameObject.SetActive(false);
    }
}
