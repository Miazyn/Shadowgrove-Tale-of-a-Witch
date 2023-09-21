using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackUnit
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }

    public void TakeDamage(int dmg);
    public void Heal(int healHP);
}
