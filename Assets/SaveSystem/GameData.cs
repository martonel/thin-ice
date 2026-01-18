using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
 
    public int levelNumber = 1;
    public bool[] stars;
    public bool[] items;
    public int[] levelScores;
    public int unlockLevels;

    public GameData()
    {
    }

    public GameData(Data data)
    {
        levelNumber = data.levelNumber;
        stars = data.stars;
        items = data.items;
        levelScores = data.levelScores;
        unlockLevels = data.unlockLevels;
    }

}
