using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    
    public int level;

    //amilyen szintû, olyan a kinézete a buborékunknak
    public int updateLevel;

    public int health;

    public PlayerData(Player player)
    {
        level = player.level;
        updateLevel = player.updateLevel;
        health = player.health;
    }
}
