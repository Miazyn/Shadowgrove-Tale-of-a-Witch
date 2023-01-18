using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "Create New NPC")]
public class SO_NPC : ScriptableObject
{
    public string NpcName;
    public SO_Voice voice;

    public NPCType Type;

    public enum NPCType
    {
        Story,
        Normal
    }

    void Awake()
    {
        Type = NPCType.Normal;    
    }
}
