using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectable : MonoBehaviour, IPointerClickHandler
{
    private Selectable selectable;
    public int characterNumber = 0;
    public CharacterSelectionManager characterSelectionManager;

    void Awake()
    {
        selectable = GetComponent<Selectable>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        characterSelectionManager.SelectCharacter(selectable, characterNumber);
    }
}
