using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/players.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }
   

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/players.data";
        if(File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream (path, FileMode.Open);
            PlayerData data = binaryFormatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("File not found");
            return null;
        }
    }

    public static void SaveData(Data data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/datas.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData gameData = new GameData(data);
        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + "/datas.data";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = binaryFormatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }
        else
        {
            GameData data = new GameData();
            data.levelNumber = 0;
            data.stars = new bool[18];
            data.items = new bool[30];
            data.levelScores = new int[10];
            data.unlockLevels = 0;
            return data;

        }
    }

}
