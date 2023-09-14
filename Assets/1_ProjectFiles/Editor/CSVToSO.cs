using UnityEngine;
using UnityEditor;
using System.IO;

public class CSVToSO
{
    private static string CSVItems = "/1_ProjectFiles/Editor/CSV/Crops.csv";
    private static string CSVSpells = "/Editor/CSV/Spells.csv";

    [MenuItem("Utilities/GenerateCrops")]
    public static void GenerateSO()
    {

        Debug.Log("Generate Items");
        string[] allLines = File.ReadAllLines(Application.dataPath + CSVItems);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(';');

            SO_Seed tester = ScriptableObject.CreateInstance<SO_Seed>();
            tester.ItemName = splitData[0];

            switch (splitData[1])
            {
                case "Spring":
                    tester.GrowthSeason[0] = SO_Seed.Season.Spring;
                    break;
                case "Summer":
                    tester.GrowthSeason[0] = SO_Seed.Season.Summer;
                    break;
                case "Autumn":
                    tester.GrowthSeason[0] = SO_Seed.Season.Fall;
                    break;
                case "Winter":
                    tester.GrowthSeason[0] = SO_Seed.Season.Winter;
                    break;
                default:
                    tester.GrowthSeason[0] = SO_Seed.Season.Spring;
                    break;
            }

            if(splitData[6] == "")
            {
                tester.BuyPrice = 100;
            }
            else
            {
                tester.BuyPrice = int.Parse(splitData[6]);
            }

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
            AssetDatabase.CreateAsset(tester, $"Assets/1_ProjectFiles/Resources/Items/{tester.ItemName}.asset");
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
