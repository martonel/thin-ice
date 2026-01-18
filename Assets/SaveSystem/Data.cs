using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{

    public int levelNumber;
    public bool[] stars;
    public bool[] items;
    public int[] levelScores;
    public int unlockLevels;


    public void SaveData()
    {
        SaveSystem.SaveData(this);
    }

    public void LoadData()
    {
        GameData gameData = SaveSystem.LoadData();
        levelNumber = gameData.levelNumber;
        stars = gameData.stars;
        items = gameData.items;
        levelScores = gameData.levelScores;
        unlockLevels = gameData.unlockLevels;
        if (gameData.items == null || gameData.items.Length ==0)
        {
            FirstLoad();
        }
    }

    public void FirstLoad()
    {
        levelNumber = 0;
        stars = new bool[18];
        items = new bool[30];
        levelScores = new int[10];
        unlockLevels = 0;
    }


}
