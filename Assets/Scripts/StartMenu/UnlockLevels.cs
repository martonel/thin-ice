using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevels : MonoBehaviour
{
    public List<Button> buttonList;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.3f);
        Data data = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Data>();
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (i <= data.unlockLevels)
            {
                buttonList[i].interactable = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
