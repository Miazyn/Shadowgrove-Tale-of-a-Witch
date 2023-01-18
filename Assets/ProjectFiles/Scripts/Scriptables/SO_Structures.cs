using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure", menuName = "SO/Item/Structure")]
public class SO_Structures : SO_Item
{
    public GameObject StructurePrefab;

    private void Awake()
    {
        TypeOfItem = ItemType.Structure;
        CanBeSold = false;
    }

    public override void Use()
    {
        Debug.Log("Trying to use Ghost Object");
        Instantiate(StructurePrefab);
    }

    public override bool CanBeUsed(GameObject _objectToInteract)
    {
        if(_objectToInteract != null)
        {
            Debug.Log($"Too close to {_objectToInteract} to build something.");
            return false;
        }
        Use();
        return true;
    }
}
