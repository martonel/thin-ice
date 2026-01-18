using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectionWatcher : MonoBehaviour
{
    [Header("UI")]
    public Button startButton;

    [Header("Character Selectables")]
    public Selectable[] characterSelectables;

    void Start()
    {
        startButton.interactable = false;
    }

    void Update()
    {
        var current = EventSystem.current.currentSelectedGameObject;

        bool characterSelected = false;

        if (current != null)
        {
            foreach (var selectable in characterSelectables)
            {
                if (current == selectable.gameObject)
                {
                    characterSelected = true;
                    break;
                }
            }
        }

        startButton.interactable = characterSelected;
    }
}
