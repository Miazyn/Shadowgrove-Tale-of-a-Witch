using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SaveData(Player player, GameManager manager)
    {
        BinaryFormatter _formatter = new BinaryFormatter();
        string _path = Application.persistentDataPath + "/saveData.save";

        FileStream _stream = new FileStream(_path, FileMode.Create);

        Data _data = new Data(player,manager);

        _formatter.Serialize(_stream, _data);
        _stream.Close();
    }
    public static Data LoadData()
    {
        string _path = Application.persistentDataPath + "/saveData.save";
        if (File.Exists(_path))
        {
            BinaryFormatter _formatter = new BinaryFormatter();
            FileStream _stream = new FileStream(_path, FileMode.Open);

            Data _data = _formatter.Deserialize(_stream) as Data;
            _stream.Close();

            return _data;
        }

        Debug.LogError("Save File not found in " + _path);
        return null;

    }
    public static bool HasSaveData()
    {
        string _path = Application.persistentDataPath + "/saveData.save";

        if (File.Exists(_path))
        {
            return true;
        }
        return false;
    }
}
