using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int damage)
    {
        currentHP -= damage;

        if(currentHP <= 0)
        {
            return true;
        }

        return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
}