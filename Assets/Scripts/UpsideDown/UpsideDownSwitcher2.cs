using UnityEngine;

public class UpsideDownSwitcher2 : MonoBehaviour
{

    public GameObject normalObj;
    public GameObject downObj;
    private bool isUpsideDown = false;

    public void TurnSide()
    {
        isUpsideDown = !isUpsideDown;
        normalObj.SetActive(!isUpsideDown);
        downObj.SetActive(isUpsideDown);
    }
}
