using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Data data;

    private int characterNumber = 0;
    

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        data = this.gameObject.GetComponent<Data>();
        data.LoadData();
    }

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event when the script is enabled
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event when the script is disabled
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("scene name:  " + scene.name);
        if (data != null)
        {
            data.SaveData();
            if (scene.name.Equals("Level2") && data.unlockLevels < 1)
            {
                data.unlockLevels = 1;
            }
            else if (scene.name.Equals("Level3") && data.unlockLevels < 2)
            {
                data.unlockLevels = 2;
            }
            else if (scene.name.Equals("Level4") && data.unlockLevels < 3)
            {
                data.unlockLevels = 3;
            }
            else if (scene.name.Equals("Level5") && data.unlockLevels < 4)
            {
                data.unlockLevels = 4;
            }
            else if (scene.name.Equals("Level6") && data.unlockLevels < 5)
            {
                data.unlockLevels = 5;
            }

            //data = this.gameObject.GetComponent<Data>();
            //data.SaveData();
        }
    }
    public void SetCharacterIndex(int index)
    {
        characterNumber = index;
    }

    public int GetCharacterIndex()
    {
        return characterNumber;
    }
    
}
