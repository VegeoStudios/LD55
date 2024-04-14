using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<string> summoned1 = new List<string>();
    public List<string> summoned2 = new List<string>();

    public List<string> responses_cooperate = new List<string>();
    public List<string> responses_imprison = new List<string>();

    public List<string> succeeded = new List<string>();
    public List<string> failed = new List<string>();
    public List<string> injured = new List<string>();

}
