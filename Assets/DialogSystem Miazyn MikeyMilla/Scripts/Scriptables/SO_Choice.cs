using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Choice", menuName = "Scriptables/Choice")]
public class SO_Choice : ScriptableObject
{
    [TextArea(5, 10)]
    public string choiceLine;
    public SO_Dialog followingDialogue;
}
