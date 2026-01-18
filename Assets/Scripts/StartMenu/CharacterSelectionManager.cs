using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;

    public Button startButton;

    private Selectable selectedCharacter;

    private int selectedCharacterIndex;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        startButton.interactable = false;
    }

    public void SelectCharacter(Selectable selectable,int characterNumber)
    {
        if (selectedCharacter != null)
            selectedCharacter.targetGraphic.color = Color.white;

        selectedCharacter = selectable;
        selectedCharacter.targetGraphic.color = Color.green;

        startButton.interactable = true;
        selectedCharacterIndex = characterNumber;
        try
        {
            GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            gameManager.SetCharacterIndex(characterNumber);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }


    }

    public bool HasSelection()
    {
        return selectedCharacter != null;
    }
}
