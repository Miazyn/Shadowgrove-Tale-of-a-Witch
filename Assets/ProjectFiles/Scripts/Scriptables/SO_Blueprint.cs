using UnityEngine;


[CreateAssetMenu(fileName = "New Blueprint", menuName = "SO/Item/Blueprint")]
public class SO_Blueprint : ScriptableObject
{
    public string BlueprintName;

    [TextArea(15, 20)]
    public string BlueprintDesc;

    public SO_Item Result;

    public SO_Item[] Materials;
    public int[] Amount;
}
