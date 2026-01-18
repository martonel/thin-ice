using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataSaver : MonoBehaviour
{
    private Data data;

    /*void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }*/

    void Start()
    {
        StartCoroutine(LateStart(0.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        SaveSystem.LoadData();
        yield return new WaitForSeconds(waitTime);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");
        data = objs[0].gameObject.GetComponent<Data>();
    }

    public void Save()
    {
        if (data != null)
        {
            SaveSystem.SaveData(data);
        }
    }

    /*public void SaveText(InputField inputField)
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            try
            {
                int level = int.Parse(inputField.text);
                data.levelNumber = level;
                Save();
            }
            catch
            {
                Debug.Log("nem sikerült a parse! Addj meg egy számot");
            }
        }
    }

    public void SaveAddCoinText(InputField inputField)
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            try
            {
                int coin = int.Parse(inputField.text);
                data.coinNumber += coin;
                Save();
            }
            catch
            {
                Debug.Log("nem sikerült a parse! Addj meg egy számot");
            }
        }
    }

    public void SaveAddXPText(InputField inputField)
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            try
            {
                int xpNumber = int.Parse(inputField.text);
                data.xpNumber += xpNumber;
                Save();
            }
            catch
            {
                Debug.Log("nem sikerült a parse! Addj meg egy számot");
            }
        }
    }*/

}
