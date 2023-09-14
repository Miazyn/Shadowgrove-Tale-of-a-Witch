using UnityEngine;
using UnityEditor;
using System.IO;

public class CSVToSO
{
    private static string CSVItems = "/Editor/CSV/Crops.csv";
    private static string CSVSpells = "/Editor/CSV/Spells.csv";

    [MenuItem("Utilities/GenerateItems")]
    public static void GenerateSO()
    {

        Debug.Log("Generate Items");
        string[] allLines = File.ReadAllLines(Application.dataPath + CSVItems);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(';');

            SO_Item tester = ScriptableObject.CreateInstance<SO_Item>();
            tester.ItemName = splitData[0];


            //var testerSprite = Resources.Load<Sprite>(resourcePath + tester.ItemName);
            //if (testerSprite != null) 
            //{
            //    tester.item = testerSprite;
            //}
            //else
            //{
            //    Debug.Log("no Sprite found at: " + resourcePath + tester.ItemName);
            //}

            //Knowledge of unity of all data //Path has alrdy to be exist
            AssetDatabase.CreateAsset(tester, $"Assets/ProjectFiles/Scriptables/Items/TestItems/{tester.ItemName}.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Utilities/Generate Spells")]
    public static void GenerateSpells()
    {

        Debug.Log("Generate Items");
        string[] allLines = File.ReadAllLines(Application.dataPath + CSVSpells);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(';');

            SO_Spell tester = ScriptableObject.CreateInstance<SO_Spell>();

            tester.SpellName = splitData[0];
            tester.Description = splitData[1];

            if (int.TryParse(splitData[2], out int manacost))
            {
                tester.ManaCost = manacost;
            }
            else
            {
                Debug.LogError($"Parsing has not worked on spell {tester.SpellName}");
            }

            Debug.Log($"Spell: {tester.SpellName}");

            AssetDatabase.CreateAsset(tester, $"Assets/ProjectFiles/Scriptables/Items/Spells/{tester.SpellName}.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
