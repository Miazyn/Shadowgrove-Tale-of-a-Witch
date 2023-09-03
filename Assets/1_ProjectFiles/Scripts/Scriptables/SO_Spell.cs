using System.Collections;
using UnityEngine;

public class SO_Spell : ScriptableObject
{

    public string SpellName;

    [TextArea(15, 20)]
    public string Description;

    public int ManaCost;
    public int Damage;
    public int HealthHealed;
    public enum StatusEffect
    {
        Confused,
        Burning,
        Frozen,
        Loyal
    }

    public StatusEffect StatusEffects;

    public int LevelReq;

    public enum AttackType
    {
        Physical,
        Magic,
        Status
    }

}
