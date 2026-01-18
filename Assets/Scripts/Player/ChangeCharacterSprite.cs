using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacterSprite : MonoBehaviour
{
    private int playerNumber = 0;

    public List<GameObject> normalCharacters;

    public List<GameObject> upsideDownCharacters;

    void Start()
    {
        try
        {
            GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            playerNumber = gameManager.GetCharacterIndex();
            for (int i = 0; i < normalCharacters.Count; i++)
            {
                if(i  == playerNumber)
                {
                    normalCharacters[i].name = "PlayerSpriteContainer";
                    normalCharacters[i].SetActive(true);
                }
                else
                {
                    normalCharacters[i].name = "PlayerSpriteContainer1";
                    normalCharacters[i].SetActive(false);
                }
            }
            for (int i = 0; i < upsideDownCharacters.Count; i++)
            {
                if (i == playerNumber)
                {
                    upsideDownCharacters[i].name = "PlayerSpriteContainer";
                    upsideDownCharacters[i].SetActive(true);
                }
                else
                {
                    upsideDownCharacters[i].name = "PlayerSpriteContainer1";
                    upsideDownCharacters[i].SetActive(false);
                }
            }


        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}
