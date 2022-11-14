using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "SO/ItemDatabase")]
public class SO_ItemDatabase : ScriptableObject
{
    public List<SO_Item> itemDatabase = new List<SO_Item>();
}
