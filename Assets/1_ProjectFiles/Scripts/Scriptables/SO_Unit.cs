using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "SO/Battle/Unit")]
public class SO_Unit : ScriptableObject
{
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
}
